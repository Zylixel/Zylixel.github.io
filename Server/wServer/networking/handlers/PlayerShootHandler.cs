#region

using System;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.entities.player;

#endregion

namespace wServer.networking.handlers
{
    internal class PlayerShootPacketHandler : PacketHandlerBase<PlayerShootPacket>
    {
        public override PacketID Id => PacketID.PLAYERSHOOT;

        protected override void HandlePacket(Client client, PlayerShootPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                if (client.Player.Owner == null) return;

                OldItem item = client.Player.Manager.GameData.Items[(ushort)packet.ContainerType];
                int stype = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (client.Player.Inventory[i]?.ObjectType == packet.ContainerType)
                    {
                        stype = i;
                        break;
                    }
                }

                if (item == Client.Player.SerialConvert(Client.Player.Inventory[1]))
                    return; // ability shoot handled by useitem implement ability check

                if (client.Player.SlotTypes[stype] != item.SlotType)
                {
                    Client.Player.kickforCheats(Player.possibleExploit.DIFF_WEAPON);
                    return;
                }
                if ((Client.Stage != ProtocalStage.Disconnected) && (!Client.Player.CheckShootSpeed(item)))
                {
                    return;
                }
                if (Client.Stage == ProtocalStage.Disconnected)
                    return;
                
                ProjectileDesc prjDesc = item.Projectiles[0]; //Assume only one
    
                Projectile prj = client.Player.PlayerShootProjectile(
                    packet.BulletId, prjDesc, item.ObjectType,
                    packet.Time, packet.Position, packet.Angle);
                client.Player.Owner.EnterWorld(prj);
                client.Player.BroadcastSync(new AllyShootPacket
                {
                    OwnerId = client.Player.Id,
                    Angle = packet.Angle,
                    ContainerType = packet.ContainerType,
                    BulletId = packet.BulletId
                }, p => p != client.Player && client.Player.Dist(p) < 25);
                client.Player.FameCounter.Shoot(prj);
            }, PendingPriority.Networking);
        }
    }
}