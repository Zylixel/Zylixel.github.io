#region

using System;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.realm.worlds;

#endregion

namespace wServer.networking.handlers
{
    internal class InvDropHandler : PacketHandlerBase<InvDropPacket>
    {
        private readonly Random _invRand = new Random();

        public override PacketID Id => PacketID.INVDROP;

        protected override void HandlePacket(Client client, InvDropPacket packet)
        {
            if (client.Player.Owner == null) return;

            if (client.Player.Owner is PetYard)
            {
                client.SendPacket(new InvResultPacket
                {
                    Result = 0
                });
            }

            client.Manager.Logic.AddPendingAction(t =>
            {
                //TODO: locker again
                const ushort normBag = 0x0500;
                const ushort soulBag = 0x0507;

                Entity entity = client.Player.Owner.GetEntity(packet.SlotObject.ObjectId);
                IContainer con = entity as IContainer;
                Item item = null;
                if (packet.SlotObject.SlotId == 254)
                {
                    client.Player.HealthPotions--;
                    item = client.Player.Manager.GameData.Items[0xa22];
                }
                else if (packet.SlotObject.SlotId == 255)
                {
                    client.Player.MagicPotions--;
                    item = client.Player.Manager.GameData.Items[0xa23];
                }
                else
                {
                    if (con.Inventory[packet.SlotObject.SlotId] == null) return;

                    item = con.Inventory[packet.SlotObject.SlotId];
                    con.Inventory[packet.SlotObject.SlotId] = null;
                }
                entity.UpdateCount++;

                if (item != null)
                {
                    Container container;
                    if (item.Secret || Client.Account.Rank == 2)
                    {
                        container = new Container(client.Player.Manager, soulBag, 1000*30, true)
                        {
                            BagOwners = new string[1] { client.Player.AccountId }
                        };
                    }
                    else
                    {
                        container = new Container(client.Player.Manager, normBag, 1000*30, true);
                    }
                    float bagx = entity.X + (float) ((_invRand.NextDouble()*2 - 1)*0.5);
                    float bagy = entity.Y + (float) ((_invRand.NextDouble()*2 - 1)*0.5);
                    try
                    {
                        container.Inventory[0] = item;
                        container.Move(bagx, bagy);
                        container.Size = 75;
                        client.Player.Owner.EnterWorld(container);

                        if (entity is Player)
                        {
                            (entity as Player).CalcBoost();
                            (entity as Player).Client.SendPacket(new InvResultPacket
                            {
                                Result = 0
                            });
                            (entity as Player).Client.Save();
                        }

                        if (client.Player.Owner is Vault)
                            if ((client.Player.Owner as Vault).PlayerOwnerName == client.Account.Name)
                                return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Console.WriteLine(client.Player.Name + " just attempted to dupe.");
                    }
                }
            }, PendingPriority.Networking);
        }
    }
}