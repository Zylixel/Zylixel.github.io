#region

using System;
using db.data;
using wServer.logic.behaviors.Drakes;

#endregion

namespace wServer.realm.setpieces
{
    internal class KeeperGodland : ISetPiece
    {
        private static readonly string Floor = "KeeperRock";
        private static readonly string Stones = "Rock Grey";

        //Ocean Rock


        private readonly Random rand = new Random();

        public int Size => 50;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            int radius = 21;

            int[,] t = new int[Size, Size];

            for (int x = 0; x < Size; x++) //Rock
                for (int y = 0; y < Size; y++)
                {
                    double dx = x - (Size / 2.0);
                    double dy = y - (Size / 2.0);
                    double r = Math.Sqrt(dx * dx + dy * dy);
                    if (r <= radius)
                    {
                        t[x, y] = 1;
                    }
                }
            for (int x = 0; x < Size; x++) //rocks
                for (int y = 0; y < Size; y++)
                {
                    double dx = x - (Size / 2.0);
                    double dy = y - (Size / 2.0);
                    double r = Math.Sqrt(dx * dx + dy * dy);
                    if (r <= radius)
                    {
                        if (rand.NextDouble() < 0.01 )
                            t[x, y] = 2;
                    }
                }

            XmlData dat = world.Manager.GameData;
            for (int x = 0; x < Size; x++) //Rendering
                for (int y = 0; y < Size; y++)
                {
                    if (t[x, y] == 1)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = dat.IdToTileType[Floor];
                        tile.ObjType = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 2)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = dat.IdToTileType[Floor];
                        tile.ObjType = dat.IdToObjectType[Stones];
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                }
            Entity god1 = Entity.Resolve(world.Manager,KeeperGodRandom.generate(1)); 
            god1.Move(pos.X + rand.Next(15,19), pos.Y + rand.Next(15, 19));
            world.EnterWorld(god1);
            Entity god2 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(2)); 
            god2.Move(pos.X + rand.Next(15, 19), pos.Y + rand.Next(10, 14));
            world.EnterWorld(god2);
            Entity god3 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(3)); 
            god3.Move(pos.X + rand.Next(15, 19), pos.Y + rand.Next(20, 24));
            world.EnterWorld(god3);
            Entity god4 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(4)); 
            god4.Move(pos.X + rand.Next(15, 19), pos.Y + rand.Next(25, 29));
            world.EnterWorld(god4);
            Entity god5 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(5));
            god5.Move(pos.X + rand.Next(15, 19), pos.Y + rand.Next(30, 34));
            world.EnterWorld(god5);
            Entity god105 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(6));
            god105.Move(pos.X + rand.Next(15, 19), pos.Y + rand.Next(35, 39));
            world.EnterWorld(god105);
            Entity god6 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(7));
            god6.Move(pos.X + rand.Next(20, 24), pos.Y + rand.Next(15, 19));
            world.EnterWorld(god6);
            Entity god7 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(8));
            god7.Move(pos.X + rand.Next(20, 24), pos.Y + rand.Next(10, 14));
            world.EnterWorld(god7);
            Entity god8 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(9));
            god8.Move(pos.X + rand.Next(20, 24), pos.Y + rand.Next(20, 24));
            world.EnterWorld(god8);
            Entity god9 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(10));
            god9.Move(pos.X + rand.Next(20, 24), pos.Y + rand.Next(25, 29));
            world.EnterWorld(god9);
            Entity god10 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(11));
            god10.Move(pos.X + rand.Next(20, 24), pos.Y + rand.Next(30, 34));
            world.EnterWorld(god10);
            Entity god110 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(12));
            god110.Move(pos.X + rand.Next(20, 24), pos.Y + rand.Next(35, 39));
            world.EnterWorld(god110);
            Entity god11 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(13));
            god11.Move(pos.X + rand.Next(25, 29), pos.Y + rand.Next(15, 19));
            world.EnterWorld(god11);
            Entity god12 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(14));
            god12.Move(pos.X + rand.Next(25, 29), pos.Y + rand.Next(10, 14));
            world.EnterWorld(god12);
            Entity god13 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(15));
            god13.Move(pos.X + rand.Next(25, 29), pos.Y + rand.Next(20, 24));
            world.EnterWorld(god13);
            Entity god14 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(16));
            god14.Move(pos.X + rand.Next(25, 29), pos.Y + rand.Next(25, 29));
            world.EnterWorld(god14);
            Entity god15 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(17));
            god15.Move(pos.X + rand.Next(25, 29), pos.Y + rand.Next(30, 34));
            world.EnterWorld(god15);
            Entity god115 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(18));
            god115.Move(pos.X + rand.Next(30, 34), pos.Y + rand.Next(35, 39));
            world.EnterWorld(god115);
            Entity god16 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(19));
            god16.Move(pos.X + rand.Next(30, 34), pos.Y + rand.Next(15, 19));
            world.EnterWorld(god16);
            Entity god17 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(20));
            god17.Move(pos.X + rand.Next(30, 34), pos.Y + rand.Next(10, 14));
            world.EnterWorld(god17);
            Entity god18 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(21));
            god18.Move(pos.X + rand.Next(30, 34), pos.Y + rand.Next(20, 24));
            world.EnterWorld(god18);
            Entity god19 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(22));
            god19.Move(pos.X + rand.Next(30, 34), pos.Y + rand.Next(25, 29));
            world.EnterWorld(god19);
            Entity god20 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(23));
            god20.Move(pos.X + rand.Next(30, 34), pos.Y + rand.Next(30, 34));
            world.EnterWorld(god20);
            Entity god120 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(24));
            god120.Move(pos.X + rand.Next(30, 34), pos.Y + rand.Next(35, 39));
            world.EnterWorld(god120);
            Entity god26 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(25));
            god26.Move(pos.X + rand.Next(34, 39), pos.Y + rand.Next(15, 19));
            world.EnterWorld(god26);
            Entity god27 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(26));
            god27.Move(pos.X + rand.Next(34, 39), pos.Y + rand.Next(10, 14));
            world.EnterWorld(god27);
            Entity god28 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(27));
            god28.Move(pos.X + rand.Next(34, 39), pos.Y + rand.Next(20, 24));
            world.EnterWorld(god28);
            Entity god29 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(28));
            god29.Move(pos.X + rand.Next(34, 39), pos.Y + rand.Next(25, 29));
            world.EnterWorld(god29);
            Entity god30 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(29));
            god30.Move(pos.X + rand.Next(34, 39), pos.Y + rand.Next(30, 34));
            world.EnterWorld(god30);
            Entity god220 = Entity.Resolve(world.Manager, KeeperGodRandom.generate(30));
            god220.Move(pos.X + rand.Next(34, 39), pos.Y + rand.Next(35, 39));
            world.EnterWorld(god220);
        }
    }
}