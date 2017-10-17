#region

using System;

#endregion

namespace terrain
{
    public enum TerrainType : byte
    {
        None,
        Mountains,
        HighSand,
        HighPlains,
        HighForest,
        MidSand,
        MidPlains,
        MidForest,
        LowSand,
        LowPlains,
        LowForest,
        ShoreSand,
        ShorePlains
    }

    public enum TileRegion : byte
    {
        None,
        Spawn,
        RealmPortals,
        Store1,
        Store2,
        Store3,
        Store4,
        Store5,
        Store6,
        Vault,
        Loot,
        Defender,
        Hallway,
        Enemy,
        Hallway1,
        Hallway2,
        Hallway3,
        Store7,
        Store8,
        Store9,
        GiftingChest,
        Store10,
        Store11,
        Store12,
        Store13,
        Store14,
        Store15,
        Store16,
        Store17,
        Store18,
        Store19,
        Store20,
        Store21,
        Store22,
        Store23,
        Store24,
        PetRegion,
        OutsideArena,
        ItemSpawnPoint,
        ArenaCentralSpawn,
        ArenaEdgeSpawn,
        Store25,
        Store26,
        Store27,
        Store28,
        Store29,
        Store30,
        Store31,
        Store32,
        Store33,
        Store34,
        Store35,
        Store36,
        Store37,
        Store38,
        Store39,
        Store40
    }

    internal struct TerrainTile : IEquatable<TerrainTile>
    {
        public string Biome;
        public float Elevation;
        public float Moisture;
        public string Name;
        public int PolygonId;
        public TileRegion Region;
        public TerrainType Terrain;
        public ushort TileId;
        public string TileObj;

        public bool Equals(TerrainTile other)
        {
            return
                TileId == other.TileId &&
                TileObj == other.TileObj &&
                Name == other.Name &&
                Terrain == other.Terrain &&
                Region == other.Region;
        }
    }
}