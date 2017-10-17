#region

using System;
using log4net;
using wServer.realm.entities.player;
using wServer.realm.setpieces;

#endregion

namespace wServer.realm.worlds
{
    internal class GameWorld : World
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (GameWorld));

        private readonly int _mapId;
        private readonly bool _oryxPresent;
        private string _displayname;

        public GameWorld(int mapId, string name, bool oryxPresent)
        {
            _displayname = name;
            Name = name;
            ClientWorldName = name;
            Background = 0;
            Difficulty = -1;
            this._oryxPresent = oryxPresent;
            this._mapId = mapId;
        }

        public Oryx Overseer { get; private set; }

        protected override void Init()
        {
            log.InfoFormat("Initializing Game World {0}({1}) from map {2}...", Id, Name, _mapId);
            LoadMap("wServer.realm.worlds.maps.world" + _mapId + ".wmap", MapType.Wmap);
            SetPieces.ApplySetPieces(this);
            if (_oryxPresent)
                Overseer = new Oryx(this);
            else
                Overseer = null;
            log.Info("Game World initalized.");
        }

        public static GameWorld AutoName(int mapId, bool oryxPresent)
        {
            string name = RealmManager.Realms[new Random().Next(RealmManager.Realms.Count)];
            RealmManager.Realms.Remove(name);
            RealmManager.CurrentRealmNames.Add(name);
            return new GameWorld(mapId, name, oryxPresent);
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);
            if (Overseer != null)
                Overseer.Tick(time);
        }

        public override int EnterWorld(Entity entity)
        {
            int ret = base.EnterWorld(entity);
            if (entity is Player)
                Overseer.OnPlayerEntered(entity as Player);
            return ret;
        }

        public override void Dispose()
        {
            if (Overseer != null)
                Overseer.Dispose();
            base.Dispose();
        }
    }
}