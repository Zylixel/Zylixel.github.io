 #region

using wServer.realm;

#endregion

namespace wServer.logic.behaviors
{
    public class ChangeGround : Behavior
    {
        private readonly int dist;
        private readonly string groundToChange;
        private readonly string targetType;

        /// <summary>
        ///     Changes the ground
        /// </summary>
        /// <param name="GroundToChange">The tiles you want to change (null for every tile)</param>
        /// <param name="ChangeTo">The tiles who will replace the old once</param>
        /// <param name="dist">The distance around the monster</param>
        public ChangeGround(string GroundToChange, string ChangeTo, int dist)
        {
            groundToChange = GroundToChange;
            targetType = ChangeTo;
            this.dist = dist;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            {
                var dat = host.Manager.GameData;
                var w = host.Owner;
                var pos = new IntPoint((int)host.X - (dist / 2), (int)host.Y - (dist / 2));
                if (w == null) return;
                for (int x = 0; x < dist; x++)
                {
                    for (int y = 0; y < dist; y++)
                    {
                        WmapTile tile = w.Map[x + pos.X, y + pos.Y].Clone();
                        if (groundToChange != null)
                        {
                            if (tile.TileId == dat.IdToTileType[groundToChange])
                            {
                                tile.TileId = dat.IdToTileType[targetType];
                                w.Map[x + pos.X, y + pos.Y] = tile;
                            }
                        }
                        else
                        {
                            int r = Random.Next(targetType.Length);
                            tile.TileId = dat.IdToTileType[targetType];
                            w.Map[x + pos.X, y + pos.Y] = tile;
                        }
                    }
                }
            };
        }
    }
}