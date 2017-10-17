#region

using System;
using db.data;

#endregion

namespace wServer.realm.setpieces
{
    internal class KeeperTomb : ISetPiece
    {
        private static readonly string Wall = "Tomb Wall";
        private static readonly string Sand = "QuickSand";
        private static readonly string Floor = "Sandstone Tile";

        
        private readonly Random rand = new Random();

        public int Size => 50;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            int radius = 18;

            int[,] t = new int[Size, Size];

            for (int x = 0; x < Size; x++) //Grassing
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
            for (int x = 0; x < Size; x++) //Grassing
                for (int y = 0; y < Size; y++)
                {
                    double dx = x - (Size / 2.0);
                    double dy = y - (Size / 2.0);
                    double r = Math.Sqrt(dx * dx + dy * dy);
                    if (r <= radius)
                    {
                        if (rand.NextDouble() < 0.40)
                            t[x, y] = 2;
                    }
                }

            XmlData dat = world.Manager.GameData;
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (t[y, x] == 1)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = dat.IdToTileType[Floor];
                        tile.ObjType = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;;
                    }
                    else if (t[y, x] == 2)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = dat.IdToTileType[Sand];
                        tile.ObjType = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[y, x] == 3)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.ObjType = dat.IdToObjectType[Wall];
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                }
            }
            Entity Bes = Entity.Resolve(world.Manager, "Keeper Defender");
            Bes.Move(pos.X + 22, pos.Y + 22);
            world.EnterWorld(Bes);
            Entity Idk = Entity.Resolve(world.Manager, "Keeper Attacker");
            Idk.Move(pos.X + 25, pos.Y + 28);
            world.EnterWorld(Idk);
            Entity Idk2 = Entity.Resolve(world.Manager, "Keeper Support");
            Idk2.Move(pos.X + 28, pos.Y + 24);
            world.EnterWorld(Idk2);
        }
    }
}