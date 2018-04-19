using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace wServer.networking
{
    internal class NetworkHandler
    {
        public const int BufferSize = 0x10000;
        private readonly Client _parent;
        private readonly ConcurrentQueue<Packet> _pendingPackets = new ConcurrentQueue<Packet>();
        private readonly object _sendLock = new object();
        private readonly Socket _skt;

        private SocketAsyncEventArgs _receive;
        public byte[] ReceiveBuff { get; private set; }
        private ReceiveState _receiveState = ReceiveState.Awaiting;

        private SocketAsyncEventArgs _send;
        private byte[] _sendBuff;
        private SendState _sendState = SendState.Awaiting;

        public NetworkHandler(Client parent, Socket skt)
        {
            _parent = parent;
            _skt = skt;
        }

        public void BeginHandling()
        {
            _skt.NoDelay = true;
            _skt.UseOnlyOverlappedIO = true;

            _send = new SocketAsyncEventArgs();
            _send.Completed += SendCompleted;
            _send.UserToken = new SendToken();
            _send.SetBuffer(_sendBuff = new byte[BufferSize], 0, BufferSize);

            _receive = new SocketAsyncEventArgs();
            _receive.Completed += ReceiveCompleted;
            _receive.UserToken = new ReceiveToken();
            _receive.SetBuffer(ReceiveBuff = new byte[BufferSize], 0, BufferSize);

            _receiveState = ReceiveState.ReceivingHdr;
            _receive.SetBuffer(0, 5);
            if (!_skt.ReceiveAsync(_receive))
                ReceiveCompleted(this, _receive);
        }

        private void ProcessPolicyFile()
        {
            var s = new NetworkStream(_skt);
            var wtr = new NWriter(s);
            wtr.WriteNullTerminatedString(
                @"<cross-domain-policy>
                    <allow-access-from domain=""*"" to-ports=""*"" />
                </cross-domain-policy>");
            wtr.Write((byte)'\r');
            wtr.Write((byte)'\n');
            _parent.Disconnect(Client.DisconnectReason.PROCESS_POLICY);
        }

        private void ReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (!_skt.Connected)
                {
                    _parent.Disconnect(Client.DisconnectReason.SKT_COMPLETED);
                    return;
                }

                if (e.SocketError != SocketError.Success)
                    throw new SocketException((int)e.SocketError);

                switch (_receiveState)
                {
                    case ReceiveState.ReceivingHdr:
                        if (e.BytesTransferred < 5)
                        {
                            _parent.Disconnect(Client.DisconnectReason.NOT_ENOUGH_BYTES);
                            return;
                        }

                        if (e.Buffer[0] == 0x3c && e.Buffer[1] == 0x70 &&
                            e.Buffer[2] == 0x6f && e.Buffer[3] == 0x6c && e.Buffer[4] == 0x69)
                        {
                            ProcessPolicyFile();
                            return;
                        }

                        int len = ((ReceiveToken) e.UserToken).Length =
                            IPAddress.NetworkToHostOrder(BitConverter.ToInt32(e.Buffer, 0)) - 5;
                        if (len < 0 || len > BufferSize)
                            Program.writeNotable("Buffer not large enough!");
                        ((ReceiveToken) e.UserToken).PacketBody = new byte[len];
                        ((ReceiveToken) e.UserToken).Id = (PacketID)e.Buffer[4];

                        _receiveState = ReceiveState.ReceivingBody;
                        e.SetBuffer(0, len);
                        _skt.ReceiveAsync(e);

                        break;
                    case ReceiveState.ReceivingBody:
                        if (e.BytesTransferred < ((ReceiveToken) e.UserToken).Length)
                        {
                            _parent.Disconnect(Client.DisconnectReason.BYTES_UNDER_RECIEVETOKEN);
                            return;
                        }

                        byte[] body = ((ReceiveToken) e.UserToken).PacketBody;
                        PacketID id = ((ReceiveToken) e.UserToken).Id;
                        Buffer.BlockCopy(e.Buffer, 0, body, 0, body.Length);

                        _receiveState = ReceiveState.Processing;
                        bool cont = OnPacketReceived(id, body);

                        if (cont && _skt.Connected)
                        {
                            _receiveState = ReceiveState.ReceivingHdr;
                            e.SetBuffer(0, 5);
                            _skt.ReceiveAsync(e);
                        }
                        break;
                    default:
                        throw new InvalidOperationException(e.LastOperation.ToString());
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private void SendCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (!_skt.Connected) return;

                int len;
                switch (_sendState)
                {
                    case SendState.Ready:
                        len = ((SendToken) e.UserToken).Packet.Write(_parent, _sendBuff, 0);

                        _sendState = SendState.Sending;
                        e.SetBuffer(0, len);
                        _skt.SendAsync(e);
                        break;
                    case SendState.Sending:
                        ((SendToken) e.UserToken).Packet = null;

                        if (CanSendPacket(e, true))
                        {
                            len = ((SendToken) e.UserToken).Packet.Write(_parent, _sendBuff, 0);

                            _sendState = SendState.Sending;
                            e.SetBuffer(0, len);
                            _skt.SendAsync(e);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }


        private void OnError(Exception ex)
        {
            Program.writeWarning($"Socket error. {ex}");
            _parent.Disconnect(Client.DisconnectReason.SOCKET_ERROR);
        }

        private bool OnPacketReceived(PacketID id, byte[] pkt)
        {
            if (!_parent.IsReady()) return false;
            _parent.Manager.Network.AddPendingPacket(_parent, id, pkt);
            return true;
        }

        private bool CanSendPacket(SocketAsyncEventArgs e, bool ignoreSending)
        {
            lock (_sendLock)
            {
                if (_sendState == SendState.Ready ||
                    (!ignoreSending && _sendState == SendState.Sending))
                    return false;
                Packet packet;
                if (_pendingPackets.TryDequeue(out packet))
                {
                    ((SendToken) e.UserToken).Packet = packet;
                    _sendState = SendState.Ready;
                    return true;
                }
                _sendState = SendState.Awaiting;
                return false;
            }
        }

        public void SendPacket(Packet pkt)
        {
            if (!_skt.Connected) return;
            _pendingPackets.Enqueue(pkt);
            if (!CanSendPacket(_send, false)) return;
            int len = ((SendToken) _send.UserToken).Packet.Write(_parent, _sendBuff, 0);

            _sendState = SendState.Sending;
            _send.SetBuffer(_sendBuff, 0, len);
            if (!_skt.SendAsync(_send))
                SendCompleted(this, _send);
        }

        public void SendPackets(IEnumerable<Packet> pkts)
        {
            if (!_skt.Connected) return;
            foreach (Packet i in pkts)
                _pendingPackets.Enqueue(i);
            if (!CanSendPacket(_send, false)) return;
            int len = ((SendToken) _send.UserToken).Packet.Write(_parent, _sendBuff, 0);

            _sendState = SendState.Sending;
            _send.SetBuffer(_sendBuff, 0, len);
            if (!_skt.SendAsync(_send))
                SendCompleted(this, _send);
        }

        private enum ReceiveState
        {
            Awaiting,
            ReceivingHdr,
            ReceivingBody,
            Processing
        }

        private class ReceiveToken
        {
            public PacketID Id;
            public int Length;
            public byte[] PacketBody;
        }

        private enum SendState
        {
            Awaiting,
            Ready,
            Sending
        }

        private class SendToken
        {
            public Packet Packet;
        }
    }
}