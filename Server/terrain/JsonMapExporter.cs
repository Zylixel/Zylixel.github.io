#region

using System.Collections.Generic;
using Ionic.Zlib;
using Newtonsoft.Json;

#endregion

namespace terrain
{
    internal class JsonMapExporter
    {
        public string Export(TerrainTile[,] tiles)
        {
            int w = tiles.GetLength(0);
            int h = tiles.GetLength(1);
            byte[] dat = new byte[w*h*2];
            int i = 0;
            Dictionary<TerrainTile, ushort> idxs = new Dictionary<TerrainTile, ushort>(new TileComparer());
            List<Loc> dict = new List<Loc>();
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    TerrainTile tile = tiles[x, y];
                    ushort idx;
                    if (!idxs.TryGetValue(tile, out idx))
                    {
                        idxs.Add(tile, idx = (ushort) dict.Count);
                        dict.Add(new Loc
                        {
                            Ground = TileTypes.Id[tile.TileId],
                            Objs = tile.TileObj == null
                                ? null
                                : new[]
                                {
                                    new Obj
                                    {
                                        Id = tile.TileObj,
                                        Name = tile.Name == null ? null : tile.Name
                                    }
                                },
                            Regions = tile.TileId == TileTypes.Beach
                                ? new[]
                                {
                                    new Obj
                                    {
                                        Id = "Spawn"
                                    }
                                }
                                : null
                        });
                    }
                    dat[i + 1] = (byte) (idx & 0xff);
                    dat[i] = (byte) (idx >> 8);
                    i += 2;
                }
            JsonDat ret = new JsonDat
            {
            };
            return JsonConvert.SerializeObject(ret);
        }

        private struct TileComparer : IEqualityComparer<TerrainTile>
        {
            public bool Equals(TerrainTile x, TerrainTile y)
            {
                return x.TileId == y.TileId && x.TileObj == y.TileObj;
            }

            public int GetHashCode(TerrainTile obj)
            {
                return obj.TileId*13 +
                       (obj.TileObj == null ? 0 : obj.TileObj.GetHashCode()*obj.Name.GetHashCode()*29);
            }
        }


        private struct JsonDat
        {
        }

        private struct Loc
        {
            public string Ground;
            public Obj[] Objs;
            public Obj[] Regions;
        }

        private struct Obj
        {
            public string Id;
            public string Name;
        }
    }
}