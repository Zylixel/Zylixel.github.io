using System;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.svrPackets;
using wServer.realm.terrain;

namespace wServer.realm.entities.player
{
    partial class Player
    {
        public int UpdatesSend { get; private set; }
        public int UpdatesReceived { get; set; }

        public const int RADIUS = 20;
        const int APPOX_AREA_OF_SIGHT = (int)(Math.PI * RADIUS * RADIUS + 1);

        int mapWidth, mapHeight;

        public HashSet<Entity> clientEntities = new HashSet<Entity>();
        HashSet<IntPoint> clientStatic = new HashSet<IntPoint>(new IntPointComparer());
        List<IntPoint> SightTiles = new List<IntPoint>();
        List<IntPoint> newSightTiles = new List<IntPoint>();

        IEnumerable<Entity> GetNewEntities()
        {
            foreach (var i in Owner.Players)
                if (clientEntities.Add(i.Value))
                    yield return i.Value;
            foreach (var i in Owner.PlayersCollision.HitTest(X, Y, RADIUS))
                if (i is Decoy || i is Pet)
                    if (clientEntities.Add(i))
                        yield return i;
            foreach (var i in Owner.EnemiesCollision.HitTest(X, Y, RADIUS))
            {
                if (i is Container)
                {
                    var owner = (i as Container).BagOwners?.Length == 1 ? (i as Container).BagOwners[0] : null;
                    if (owner != null && owner != AccountId) continue;

                    if (owner == AccountId)
                        if ((LootDropBoost || LootTierBoost) && (i.ObjectType != 0x500 || i.ObjectType != 0x506))
                            (i as Container).BoostedBag = true; //boosted bag

                }

                if (SightTiles.Contains(new IntPoint((int)i.X, (int)i.Y)))
                    if (MathsUtils.DistSqr(i.X, i.Y, X, Y) <= RADIUS * RADIUS)
                        if (clientEntities.Add(i))
                            yield return i;
            }
            if (Quest != null && clientEntities.Add(Quest))
                yield return Quest;
        }

        IEnumerable<int> GetRemovedEntities()
        {
            foreach (var i in clientEntities)
            {
                if (!SightTiles.Contains(new IntPoint((int)i.X, (int)i.Y)) && !(i is StaticObject && (i as StaticObject).Static) && i != Quest && !(i is Player) && !(i is Pet))
                    yield return i.Id;
                else if (i.Owner == null)
                    yield return i.Id;
            }
        }

        IEnumerable<ObjectDef> GetNewStatics(int _x, int _y)
        {
            List<ObjectDef> ret = new List<ObjectDef>();
            foreach (var i in SightTiles)
            {
                var x = i.X;
                var y = i.Y;

                if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight)
                    continue;

                var tile = Owner.Map[x, y];
                if (tile.ObjId != 0 && tile.ObjType != 0 && clientStatic.Add(new IntPoint(x, y)))
                    ret.Add(tile.ToDef(x, y));
            }
            return ret;
        }

        IEnumerable<IntPoint> GetRemovedStatics(int _x, int _y)
        {
            foreach (var i in clientStatic)
            {
                var dx = i.X;
                var dy = i.Y;
                var tile = Owner.Map[i.X, i.Y];
                if (tile.ObjType == 0)
                {
                    int objId = Owner.Map[i.X, i.Y].ObjId;
                    if (objId != 0)
                        yield return i;
                }
            }
        }

        Dictionary<Entity, int> lastUpdate = new Dictionary<Entity, int>();
        void SendUpdate(RealmTime time)
        {
            int _x = (int)X;
            int _y = (int)Y;
            if (Owner == null) return;
            SightTiles = (Owner.IsBlocked() ? Sight.RayCast(this, RADIUS) : Sight.GetSightCircle(this, RADIUS).ToList());

            mapWidth = Owner.Map.Width;
            mapHeight = Owner.Map.Height;
        
            var sendEntities = new HashSet<Entity>(GetNewEntities());

            var list = new List<UpdatePacket.TileData>(APPOX_AREA_OF_SIGHT);
            int sent = 0;
            foreach (var i in SightTiles)
            {
                int x = i.X;
                int y = i.Y;

                WmapTile tile = Owner.Map[x, y];
                if (x < 0 || x >= mapWidth ||
                    y < 0 || y >= mapHeight ||
                    _tiles[x, y] >= (tile = Owner.Map[x, y]).UpdateCount) continue;
                
                list.Add(new UpdatePacket.TileData()
                {
                    X = (short)x,
                    Y = (short)y,
                    Tile = tile.TileId
                });
                _tiles[x, y] = tile.UpdateCount;
                sent++;
            }
            foreach (var i in newSightTiles)
                SightTiles.Add(i);
            FameCounter.TileSent(sent);

            var dropEntities = GetRemovedEntities().Distinct().ToArray();
            clientEntities.RemoveWhere(_ => Array.IndexOf(dropEntities, _.Id) != -1);

            foreach (var i in sendEntities)
                lastUpdate[i] = i.UpdateCount;

            var newStatics = GetNewStatics(_x, _y).ToArray();
            var removeStatics = GetRemovedStatics(_x, _y).ToArray();
            List<int> removedIds = new List<int>();
            foreach (var i in removeStatics)
            {
                removedIds.Add(Owner.Map[i.X, i.Y].ObjId);
                clientStatic.Remove(i);
            }

            if (sendEntities.Count > 0 || list.Count > 0 || dropEntities.Length > 0 || newStatics.Length > 0 || removedIds.Count > 0)
            {
                Client.SendPacket(new UpdatePacket()
                {
                    Tiles = list.ToArray(),
                    NewObjects = sendEntities.Select(_ => _.ToDefinition()).Concat(newStatics).ToArray(),
                    RemovedObjectIds = dropEntities.Concat(removedIds).ToArray()
                });
            }
            SendNewTick(time);
        }

        int tickId = 0;
        void SendNewTick(RealmTime time)
        {
            var sendEntities = new List<Entity>();
            foreach (var i in clientEntities.Where(i => i.UpdateCount > lastUpdate[i]))
            {
                sendEntities.Add(i);
                lastUpdate[i] = i.UpdateCount;
            }

            if (Quest != null && (!lastUpdate.ContainsKey(Quest) || Quest.UpdateCount > lastUpdate[Quest]))
            {
                sendEntities.Add(Quest);
                lastUpdate[Quest] = Quest.UpdateCount;
            }

            Client.SendPacket(new NewTickPacket()
            {
                TickId = tickId++,
                TickTime = time.thisTickTimes,
                UpdateStatuses = sendEntities.Select(_ => _.ExportStats()).ToArray()
            });

            SightTiles.Clear();
        }
    }
}