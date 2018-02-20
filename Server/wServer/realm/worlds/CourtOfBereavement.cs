#region

using System;
using System.Collections.Generic;
using wServer.networking;
using wServer.networking.svrPackets;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.realm.setpieces;

#endregion

namespace wServer.realm.worlds
{
    public class CourtOfBereavement : World
    {
        private bool _ready;
        private bool _waitforplayers = true;
        private bool _waiting;
        public int Wave = 1;
        public static bool canJoin = true;

        public CourtOfBereavement()
        {
            Name = "Court Of Bereavement";
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.arena.wmap", MapType.Wmap);

            canJoin = true;

            Timers.Add(new WorldTimer(60000, (world, t) =>
            {
                _waitforplayers = false;
                canJoin = false;
            }));
            
            InformPlayers(true);
        }

        private readonly string[] _gods =
        {
            "Beholder", "Ent God", "Flying Brain", "Djinn", "Ghost God", "Leviathan", "Medusa", "Slime God", "Sprite God", "White Demon"
        };
        private readonly string[] _bosses =
        {
            "Archdemon Malphas", "Limon the Sprite God",
            "Lord Ruthven", "Septavius the Ghost God", "Stheno the Snake Queen", "Arachna the Spider Queen", "Oryx the Mad God 1"
        };
        private readonly string[] _bosses2 =
        {
            "Tomb Defender", "Tomb Attacker", "Tomb Support", "Crystal Prisoner", "Grand Sphinx", 
            "Oryx the Mad God 2", "Thessal the Mermaid Goddess"
        };
        private readonly string[] _bosses3 =
        {
            "shtrs the forgotten king", "shtrs Bridge Sentinel", "Murderous Megamoth", "Son of Arachna",
            "Oryx the Mad God 3", //"Keeper Boss Anchor"
        };

        public bool OutOfBounds(float x, float y)
        {
            if (Map.Height >= y && Map.Width >= x && x > -1 && y > 0)
                return Map[(int)x, (int)y].Region == TileRegion.Outside_Arena;
            return true;
        }
        
        public void InformPlayers(bool firstTime)
        {
            if (firstTime)
            {
                foreach (Client i in Manager.Clients.Values)
                {
                    i.SendPacket(new TextPacket
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "@ANNOUNCEMENT",
                        Text = "A interdimensional portal to the Court Of Bereavement has opened! Type /court to join!"
                    });
                }
            }

            if (_waitforplayers)
            {
                foreach (KeyValuePair<int, Player> i in Players)
                {
                    i.Value.Client.SendPacket(new TextPacket
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "Court Overseer",
                        Text = "The battle will soon commence once the dungeon has closed"
                    });
                }
            }
            Timers.Add(new WorldTimer(15000, (world, t) =>
            {
                InformPlayers(false);
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
                                Name = "Court Overseer",
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

                for (int i = 0; i < Wave/2 + 1; i++)
                {
                    enems.Add(_gods[r.Next(0, _gods.Length)]);
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
                if (Wave > 10 && Wave < 20)
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
                Console.WriteLine(ex);
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