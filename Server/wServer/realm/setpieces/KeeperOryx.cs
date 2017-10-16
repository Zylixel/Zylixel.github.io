#region

using System;
using db.data;
using terrain;

#endregion

namespace wServer.realm.setpieces
{
    internal class KeeperOryx : ISetPiece
    {
        private static readonly string Floor = "Blue Closed"; // 1
        private static readonly string Floor2 = "Blue Quad"; // 2
        private static readonly string Wall = "Blue Wall"; // 3
        
        private readonly Random rand = new Random();

        public int Size
        {
            get { return 50; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            int radius = 18;



            int[,] t = new int[Size, Size];

            for (int x = 0; x < Size; x++) //Floor
                for (int y = 0; y < Size; y++)
                {
                    double dx = x - (Size / 2.0);
                    double dy = y - (Size / 2.0);
                    double r = Math.Sqrt(dx * dx + dy * dy);
                    if (r <= radius)
                    {
                        if (rand.NextDouble() < 0.5)
                        {
                            t[x, y] = 1;
                        }
                        else
                        {
                            t[x, y] = 2;
                        }
                    }
                }
            for (int x = 0; x < Size; x++) //Wall
                for (int y = 0; y < Size; y++)
                {
                    double dx = x - (Size / 2.0);
                    double dy = y - (Size / 2.0);
                    double r = Math.Sqrt(dx * dx + dy * dy);
                    if (r <= radius + 1)
                        if (r >= radius - 1)
                        {
                        t[x, y] = 3;
                        }
                }


            XmlData dat = world.Manager.GameData;
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    double dx = x - (Size / 2.0);
                    double dy = y - (Size / 2.0);
                    double r = Math.Sqrt(dx * dx + dy * dy);
                    if (r <= radius)
                        if (t[y, x] == 1)
                        {
                            WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                            tile.TileId = dat.IdToTileType[Floor];
                            tile.ObjType = 0;
                            world.Map[x + pos.X, y + pos.Y] = tile;
                        }
                    if (t[y, x] == 2)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = dat.IdToTileType[Floor2];
                        tile.ObjType = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    if (r <= radius)
                        if (t[y, x] == 3)
                        {
                            WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                            tile.TileId = dat.IdToTileType[Floor];
                            tile.ObjType = dat.IdToObjectType[Wall];
                            if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                            world.Map[x + pos.X, y + pos.Y] = tile;
                        }
                }
            }
        }
    }
}
