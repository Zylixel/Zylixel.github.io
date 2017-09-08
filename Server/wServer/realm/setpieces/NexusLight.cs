#region

using System;
using db.data;
using terrain;
using wServer.logic.behaviors.Drakes;

#endregion

namespace wServer.realm.setpieces
{
    internal class NexusDark : ISetPiece
    {
        private static readonly string ToFloor = "Bright Grass";
        private static readonly string ToOcean = "Dark Water Flowing";
        private static readonly string ToShallow = "Water";
        private static readonly string Floor = "Light Grass";
        private static readonly string Ocean = "Tropical Water Deep";
        private static readonly string Shallow = "Tropical Water Surf";
        private static readonly string F1 = "Bush";
        private static readonly string F2 = "Butterfly";
        private static readonly string F4 = "Fir Tree 2";

        private readonly Random rand = new Random();

        public int Size
        {
            get { return 75; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            int[,] t = new int[Size, Size];

            for (int x = 0; x < Size; x++) //Bush
                for (int y = 0; y < Size; y++)
                {
                    {
                        if (rand.NextDouble() < 0.03)
                            t[x, y] = 1;
                    }
                }
            for (int x = 0; x < Size; x++) //Butterfly
                for (int y = 0; y < Size; y++)
                {
                    {
                        if (rand.NextDouble() < 0.03)
                            t[x, y] = 2;
                    }
                }
            for (int x = 0; x < Size; x++) //Fir Tree 2
                for (int y = 0; y < Size; y++)
                {
                    {
                        if (rand.NextDouble() < 0.03)
                            t[x, y] = 4;
                    }
                }

            XmlData dat = world.Manager.GameData;
            for (int x = 0; x < Size; x++) //Rendering
                for (int y = 0; y < Size; y++)
                {
                    if (t[x, y] != 100)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        if (tile.TileId == dat.IdToTileType[Floor])
                        {
                            tile.ObjType = 0;
                            if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                            tile.TileId = dat.IdToTileType[ToFloor];
                        }
                        if (tile.TileId == dat.IdToTileType[Ocean])
                        {
                            tile.TileId = dat.IdToTileType[ToOcean];
                        }
                        if (tile.TileId == dat.IdToTileType[Shallow])
                        {
                            tile.TileId = dat.IdToTileType[ToShallow];
                        }
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    if (t[x, y] == 1)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        if (tile.TileId == dat.IdToTileType[ToFloor])
                        {
                            tile.ObjType = dat.IdToObjectType[F1];
                            if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        }
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    if (t[x, y] == 2)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        if (tile.TileId == dat.IdToTileType[ToFloor])
                        {
                            tile.ObjType = dat.IdToObjectType[F2];
                            if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        }
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    if (t[x, y] == 4)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        if (tile.TileId == dat.IdToTileType[ToFloor])
                        {
                            tile.ObjType = dat.IdToObjectType[F4];
                            if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        }
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                }
        }
    }
}