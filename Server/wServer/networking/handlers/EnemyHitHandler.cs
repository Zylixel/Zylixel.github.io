#region

using System;
using wServer.networking.cliPackets;
using wServer.realm;
using wServer.realm.entities;

#endregion

namespace wServer.networking.handlers
{
    internal class EnemyHitHandler : PacketHandlerBase<EnemyHitPacket>
    {
        public override PacketID Id => PacketID.ENEMYHIT;

        protected override void HandlePacket(Client client, EnemyHitPacket packet)
        {
            if (client.Player.Owner == null) return;

            client.Manager.Logic.AddPendingAction(t =>
            {
                if (client.Player.Owner == null)
                    return;

                Entity entity = client.Player.Owner.GetEntity(packet.TargetId);
                if (entity != null) //Tolerance
                {
                    Projectile prj = (client.Player as IProjectileOwner).Projectiles[packet.BulletId];
                    if (client.Player.Inventory[3] != null && client.Player.Inventory[3].ObjectId == "Ogmurs Hope" && new Random().NextDouble() <= 0.05)
                    {
                        client.Player.ApplyConditionEffect(new ConditionEffect
                        {
                            DurationMS = 500,
                            Effect = ConditionEffectIndex.Damaging
                        });
                    }
                    if (prj != null)
                        prj.ForceHit(entity, t);
                }
            }, PendingPriority.Networking);
        }
    }
}