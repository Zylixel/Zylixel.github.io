#region

using System;
using db.data;
using terrain;
using wServer.logic.behaviors.Drakes;

#endregion

namespace wServer.realm.setpieces
{
    internal class NewWorldWaterFill : ISetPiece
    {
        private static readonly string F1 = "SN Wen Br";
        private static readonly string F2 = "SN Bh Sd";
        private static readonly string F3 = "SN Wen Br TRE";
        private static readonly string F4 = "SN Wen Br TRIE";
        private static readonly string F5 = "SN Wen Br RE";
        private static readonly string F6 = "SN Wen Br BRE";
        private static readonly string F7 = "SN Wen Br BRIE";
        private static readonly string F8 = "SN Wen Br BE";
        private static readonly string F9 = "SN Wen Br BLE";
        private static readonly string F10 = "SN Wen Br BLIE";
        private static readonly string F11 = "SN Wen Br LE";
        private static readonly string F12 = "SN Wen Br TLE";
        private static readonly string F13 = "SN Wen Br TLIE";
        private static readonly string F14 = "SN Wen Br TE";
        private static readonly string F15 = "SN Bh Sd Sf";
        private static readonly string Ocean = "SNR s S W Dk D";

        public int Size
        {
            get { return 230; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            int[,] t = new int[Size, Size];

            for (int x = 0; x < Size; x++) //Bush
                for (int y = 0; y < Size; y++)
                {
                    {
                            t[x, y] = 1;
                    }
                }

            XmlData dat = world.Manager.GameData;
            for (int x = 0; x < Size; x++) //Rendering
                for (int y = 0; y < Size; y++)
                {
                    if (t[x, y] != 100)
                    {
                        WmapTile tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        if (!(tile.TileId == dat.IdToTileType[F1]))
                            if (!(tile.TileId == dat.IdToTileType[F2]))
                                if (!(tile.TileId == dat.IdToTileType[F3]))
                                    if (!(tile.TileId == dat.IdToTileType[F4]))
                                        if (!(tile.TileId == dat.IdToTileType[F5]))
                                            if (!(tile.TileId == dat.IdToTileType[F6]))
                                                if (!(tile.TileId == dat.IdToTileType[F7]))
                                                    if (!(tile.TileId == dat.IdToTileType[F8]))
                                                        if (!(tile.TileId == dat.IdToTileType[F9]))
                                                            if (!(tile.TileId == dat.IdToTileType[F10]))
                                                                if (!(tile.TileId == dat.IdToTileType[F11]))
                                                                    if (!(tile.TileId == dat.IdToTileType[F12]))
                                                                        if (!(tile.TileId == dat.IdToTileType[F13]))
                                                                            if (!(tile.TileId == dat.IdToTileType[F14]))
                                                                                if (!(tile.TileId == dat.IdToTileType[F15]))
                                                                                {
                            tile.TileId = dat.IdToTileType[Ocean];
                        }
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                }
        }
    }
}