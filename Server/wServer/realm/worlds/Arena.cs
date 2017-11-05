#region

using System;
using System.Collections.Generic;
using wServer.networking.svrPackets;
using wServer.realm.entities;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.worlds
{
    public class Arena : World
    {
        private bool _ready;
        private bool _waitforplayers = true;
        private bool _waiting;
        public int Wave = 1;

        public Arena()
        {
            Id = ARENA;
            Name = "Arena";
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.arena.wmap", MapType.Wmap);

            Timers.Add(new WorldTimer(60000, (world, t) =>
            {
                _waitforplayers = false;
            }));
            
            InformPlayers();
        }

        private readonly string[] _normalEnemies =
        {
            "Aberrant of Oryx", "Abomination of Oryx", "Assassin of Oryx", "Bile of Oryx", "Gigacorn",
            "Monstrosity of Oryx", "Phoenix Reborn", "Shambling Sludge", "Urgle"
        };
        private readonly string[] _gods =
        {
            "Beholder", "Ent God", "Flying Brain", "Djinn", "Ghost God", "Leviathan", "Medusa", "Slime God", "Sprite God", "White Demon"
        };
        private readonly string[] _bosses =
        {
            "Archdemon Malphas", "Limon the Sprite God",
            "Lord Ruthven", "Septavius the Ghost God", "Stheno the Snake Queen",  "Arachna the Spider Queen"
        };
        private readonly string[] _bosses2 =
        {
            "Tomb Defender", "Tomb Attacker", "Tomb Support", "Crystal Prisoner", "Grand Sphinx", 
            "Oryx the Mad God 2", "Thessal the Mermaid Goddess"
        };
        private readonly string[] _bosses3 =
        {
            "shtrs the forgotten king", "shtrs Bridge Sentinel",
            "Oryx the Mad God 3", "Keeper Boss Anchor"
        };

        public bool OutOfBounds(float x, float y)
        {
            if (Map.Height >= y && Map.Width >= x && x > -1 && y > 0)
                return (Map[(int)x, (int)y].Region == TileRegion.Outside_Arena);
            return true;
        }

        public void InformPlayers()
        {
            if (_waitforplayers)
            {
                foreach (KeyValuePair<int, Player> i in Players)
                {
                    i.Value.Client.SendPacket(new TextPacket
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "Arena Overseer",
                        Text = "The battle will soon commence once the dungeon has closed"
                    });
                }
            }
            Timers.Add(new WorldTimer(60000, (world, t) =>
            {
                InformPlayers();
            }));
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);
            CheckOutOfBounds();

            if (!_waitforplayers)
            {
                if (Enemies.Count == 0)
                {
                    if (_ready)
                    {
                        if (_waiting) return;
                        _ready = false;
                        Wave++;
                        foreach (KeyValuePair<int, Player> i in Players)
                        {
                            i.Value.Client.SendPacket(new ArenaNextWavePacket
                            {
                                Type = Wave
                            });
                            i.Value.Client.SendPacket(new TextPacket
                            {
                                BubbleTime = 0,
                                Stars = -1,
                                Name = "Arena Overseer",
                                Text = "The next wave will start in 5 seconds"
                            });
                        }
                        _waiting = true;
                        Timers.Add(new WorldTimer(5000, (world, t) =>
                        {
                            _ready = false;
                            Spawn();
                            _waiting = false;
                        }));
                    }
                    _ready = true;
                }
            }
        }

        private void Spawn()
        {
            try
            {
                List<string> enems = new List<string>();
                Random r = new Random();

                for (int i = 0; i < Wave/3 + 1; i++)
                {
                    enems.Add(_gods[r.Next(0, _gods.Length)]);
                }
                for (int i = 0; i < Wave/3 + 1; i++)
                {
                    enems.Add(_normalEnemies[r.Next(0, _normalEnemies.Length)]);
                }

                Random r2 = new Random();
                foreach (string i in enems)
                {
                    ushort id = Manager.GameData.IdToObjectType[i];
                    int xloc = r2.Next(10, Map.Width) - 6;
                    int yloc = r2.Next(10, Map.Height) - 6;
                    Entity enemy = Entity.Resolve(Manager, id);
                    enemy.Move(xloc, yloc);
                    EnterWorld(enemy);
                }

                if (Wave < 11)
                {
                    List<string> boss = new List<string> { _bosses[r.Next(0, _bosses.Length)] };
                    foreach (string i in boss)
                    {
                        ushort id = Manager.GameData.IdToObjectType[i];
                        int xloc = Map.Width / 2;
                        int yloc = Map.Height / 2;
                        Entity enemy = Entity.Resolve(Manager, id);
                        enemy.Move(xloc, yloc);
                        EnterWorld(enemy);
                    }
                }
                if (Wave >= 10 && Wave < 19)
                {
                    List<string> boss = new List<string> {_bosses2[r.Next(0, _bosses2.Length)]};
                    foreach (string i in boss)
                    {
                        ushort id = Manager.GameData.IdToObjectType[i];
                        int xloc = Map.Width / 2;
                        int yloc = Map.Height / 2;
                        Entity enemy = Entity.Resolve(Manager, id);
                        enemy.Move(xloc, yloc);
                        EnterWorld(enemy);
                    }
                }
                if (Wave >= 20)
                {
                    List<string> boss = new List<string> { _bosses3[r.Next(0, _bosses3.Length)] };
                    foreach (string i in boss)
                    {
                        ushort id = Manager.GameData.IdToObjectType[i];
                        int xloc = Map.Width / 2;
                        int yloc = Map.Height / 2;
                        Entity enemy = Entity.Resolve(Manager, id);
                        enemy.Move(xloc, yloc);
                        EnterWorld(enemy);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void CheckOutOfBounds()
        {
            foreach (KeyValuePair<int, Enemy> i in Enemies)
            {
                if (OutOfBounds(i.Value.X, i.Value.Y))
                {
                    LeaveWorld(i.Value);
                }
            }
        }
    }
}