#region

using wServer.realm;

#endregion

namespace wServer.logic.behaviors
{
    public class OpenGate : Behavior
    {
        private int xMin;
        private int xMax;
        private int yMin;
        private int yMax;

        private string target;
        private int area;

        private bool usearea;

        public OpenGate(int xMin, int xMax, int yMin, int yMax)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;
        }

        public OpenGate(string target, int area = 10)
        {
            this.target = target;
            this.area = area;
            usearea = true;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (usearea)
            {
                for (int x = (int)host.X - area; x <= (int)host.X + area; x++)
                {
                    for (int y = (int)host.Y - area; y <= (int)host.Y + area; y++)
                    {
                        WmapTile tile = host.Owner.Map[x, y];
                        if (tile.ObjType == host.Manager.GameData.IdToObjectType[target])
                        {
                            tile.ObjType = 0;
                            host.Owner.Map[x, y] = tile;
                        }
                    }
                }
            }
            else
            {
                for (int x = xMax; x <= xMax; x++)
                {
                    for (int y = yMin; y <= yMax; y++)
                    {
                        WmapTile tile = host.Owner.Map[x, y];
                        tile.ObjType = 0;
                        host.Owner.Map[x, y] = tile;
                    }
                }
            }
        }
    }
}
