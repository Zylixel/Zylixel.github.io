﻿#region

//using wServer.logic;
using System;
using System.Collections.Generic;
using Mono.Game;
using wServer.networking.svrPackets;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.entities
{
    internal class Decoy : StaticObject, IPlayer
    {
        private static readonly Random rand = new Random();

        private readonly int duration;
        private readonly Player player;
        private readonly float speed;
        private Vector2 direction;
        private bool exploded;
        private int explode;

        public Decoy(RealmManager manager, Player player, int duration, float tps, int random, int explode)
            : base(manager, 0x0715, duration, true, true, true)
        {
            this.explode = explode;
            this.player = player;
            this.duration = duration;
            speed = tps;
            
            Position? history = player.TryGetHistory(100);
            if (history == null)
              direction = GetRandDirection();
            if (random == 1)
                direction = GetRandDirection();
            else
            {
                direction = new Vector2(player.X - history.Value.X, player.Y - history.Value.Y);
                if (direction.LengthSquared() == 0)
                    direction = GetRandDirection();
                else
                    direction.Normalize();
            }

        }

        public void Damage(int dmg, Entity chr) { }

        public bool IsVisibleToEnemy() { return true; }

        private Vector2 GetRandDirection()
        {
            double angle = rand.NextDouble()*2*Math.PI;
            return new Vector2(
                (float) Math.Cos(angle),
                (float) Math.Sin(angle)
                );
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.Texture1] = player.Texture1;
            stats[StatsType.Texture2] = player.Texture2;
            base.ExportStats(stats);
        }

        public override void Tick(RealmTime time)
        {
            if (HP > duration / 2)
            {
                ValidateAndMove(
                    X + direction.X * speed * time.thisTickTimes / 1000,
                    Y + direction.Y * speed * time.thisTickTimes / 1000
                );
            }
            if (HP < 250 && !exploded)
            {
                exploded = true;
                Entity entity = Resolve(player.Manager, 0x6148);
                entity.Move(X, Y);
                Owner.EnterWorld(entity);
                Owner.BroadcastPacket(new ShowEffectPacket
                {
                    EffectType = EffectType.AreaBlast,
                    Color = new ARGB(0xffff0000),
                    TargetId = Id,
                    PosA = new Position { X = 1 }
                }, null);
            }
            base.Tick(time);
        }
    }
}