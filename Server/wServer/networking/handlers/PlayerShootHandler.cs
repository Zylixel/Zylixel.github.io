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

        public void CheckShootSpeed(Item item)
        {
            if (Client.Player.Stats[7] > 110) return; //Becomes Unstable at that point
            Client.Player.shootCounter++;

            float diff = (Environment.TickCount - Client.Player.lastShootTime);
            if (diff < Client.Player.Stats[7] * 1.75)
            {
                if (Client.Player.shootCounter > (item.NumProjectiles * item.RateOfFire))
                {
                    Client.Player.shootCounter = 0;
                    Client.Player.checkForDex++;
                    Client.Player.Owner.Timers.Add(new WorldTimer(200, (world, t) =>
                    {
                        if (Client.Player == null) return;
                        if (Client.Player.checkForDex > 0)
                            Client.Player.checkForDex--;
                        return;
                    }));
                }
            }
            else
            {
                Client.Player.shootCounter = 0;
                Client.Player.checkForDex--;
                Client.Player.lastShootTime = Environment.TickCount;
            }
        }

        protected override void HandlePacket(Client client, PlayerShootPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                if (client.Player.Owner == null) return;

                Item item = client.Player.Manager.GameData.Items[(ushort)packet.ContainerType];
                int stype = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (client.Player.Inventory[i]?.ObjectType == packet.ContainerType)
                    {
                        stype = i;
                        break;
                    }
                }

                if (client.Player.SlotTypes[stype] != item.SlotType && client.Account.Rank < 2)
                {
                    Console.WriteLine($"{client.Player.Name} is trying to cheat (Weapon doesnt match the slot type)");
                    client.Player.SendError("This cheating attempt has been logged and a message was send to all online admins.");
                    client.Disconnect();
                    foreach (Player player in client.Player.Owner.Players.Values)
                        if (player.Client.Account.Rank >= 2)
                            player.SendInfo($"Player {client.Player.Name} is shooting with a weapon that doesnt match the class slot type.");
                    return;
                }
                CheckShootSpeed(item);
                    
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
