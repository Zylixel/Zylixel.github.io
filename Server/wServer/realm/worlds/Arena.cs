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
    public class Arena : World
    {
        public static bool _waitforplayers = true;
        private bool _waiting;
        public int Wave = 0;
        private List<IntPoint> _outerSpawn = new List<IntPoint>();
        private List<IntPoint> _centralSpawn = new List<IntPoint>();

        public Arena()
        {
            Id = ARENA;
            Name = "Arena";
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap($"wServer.realm.worlds.maps.arena{new Random().Next(1,2)}.jm", MapType.Json);
            Wave = 0;
            _waitforplayers = true;

            // setup spawn regions
            for (int x = 0; x < Map.Width; x++)
                for (int y = 0; y < Map.Height; y++)
                {
                    if (Map[x, y].Region == TileRegion.Arena_Central_Spawn)
                        _centralSpawn.Add(new IntPoint(x, y));

                    if (Map[x, y].Region == TileRegion.Arena_Edge_Spawn)
                        _outerSpawn.Add(new IntPoint(x, y));
                }

            Timers.Add(new WorldTimer(5000, (world, t) =>
            {
                _waitforplayers = false;
            }));
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
        private readonly string[] _bosses10 =
        {
            "Tomb Defender", "Tomb Attacker", "Tomb Support", "Crystal Prisoner", "Grand Sphinx",
            "Oryx the Mad God 2", "Thessal the Mermaid Goddess"
        };
        private readonly string[] _bosses20 =
        {
            "shtrs the forgotten king", "shtrs Bridge Sentinel", "Murderous Megamoth", "Son of Arachna",
            "Oryx the Mad God 3"
        };

        public bool OutOfBounds(float x, float y)
        {
            if (Map.Height >= y && Map.Width >= x && x > -1 && y > 0)
                return Map[(int)x, (int)y].Region == TileRegion.Outside_Arena;
            return true;
        }

        private void SendMsg(KeyValuePair<int, Player> player, string message, string src = "")
        {
            player.Value.Client.SendPacket(new TextPacket
            {
                Name = src,
                ObjectId = -1,
                Stars = -1,
                BubbleTime = 0,
                Recipient = "",
                Text = message,
                CleanText = ""
            });
        }

        private readonly string[] _low =
        {
            "Undead Lair Key", "Snake Pit Key", "Sprite World Key", "Mad Lab Key", "Abyss of Demons Key"
        };
        private readonly string[] _medium =
        {
            "Candy Key", "Cemetery Key", "Bella's Key", "Davy's Key", "Woodland Labyrinth Key", "The Crawling Depths Key"
        };

        private void giftItems(string[] list)
        {
            foreach (KeyValuePair<int, Player> i in Players)
            {
                for (var r = 4; r < i.Value.Inventory.Length; r++)
                {
                    if (i.Value.Inventory[r] == null)
                    {
                        ushort Item = i.Value.Manager.GameData.IdToObjectType[list[new Random(trueRandom()).Next(0, list.Length)]];
                        i.Value.Inventory[r] = i.Value.Manager.GameData.Items[Item];
                        SendMsg(i, "Congratulations on making it this far, here's a reward for you!", "^Arena Overseer");
                        break;
                    }
                    if (r == i.Value.Inventory.Length - 1)
                        SendMsg(i, "Although you made it this far, I couldn't give you a reward because your inventory was full!", "^Arena Overseer");
                }
            }
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);
            CheckOutOfBounds();

            if (!_waitforplayers)
            {
                if (Enemies.Count == 0)
                {
                    if (!_waiting)
                    {
                        _waiting = true;
                        Wave++;
                        foreach (KeyValuePair<int, Player> i in Players)
                        {
                            i.Value.Client.SendPacket(new ArenaNextWavePacket
                            {
                                Type = Wave
                            });
                            switch (Wave)
                            {
                                case 5: giftItems(_low); break;
                                case 10: SendMsg(i, "Let's kick the difficulty up a notch!", "^Arena Overseer"); break;
                                case 15: giftItems(_medium); break;
                                case 25: SendMsg(i, "Let's kick the difficulty up even more shall we?", "^Arena Overseer"); break;
                            }
                            SendMsg(i, "The next wave will start in 5 seconds", "^Arena Overseer");
                        }
                        Timers.Add(new WorldTimer(5000, (world, t) =>
                        {
                            Spawn();
                            _waiting = false;
                        }));
                    }
                }
            }
        }

        private void SpawnBoss(string[] list)
        {
            Random r = new Random();
            ushort id = Manager.GameData.IdToObjectType[list[r.Next(0, list.Length)]];
            var pos = _centralSpawn[r.Next(0, _centralSpawn.Count)];
            var xloc = pos.X + 0.5f;
            var yloc = pos.Y + 0.5f;
            Entity enemy = Entity.Resolve(Manager, id);
            enemy.Move(xloc, yloc);
            EnterWorld(enemy);
        }

        private void Spawn()
        {
            try
            {
                List<string> enems = new List<string>();
                Random r = new Random();

                for (int i = 0; i < Wave + 1; i++)
                {
                    enems.Add(_gods[r.Next(0, _gods.Length)]);
                }

                Random r2 = new Random();
                foreach (string i in enems)
                {
                    ushort id = Manager.GameData.IdToObjectType[i];
                    var pos = _outerSpawn[r.Next(0, _outerSpawn.Count)];
                    var xloc = pos.X + 0.5f;
                    var yloc = pos.Y + 0.5f;
                    Entity enemy = Entity.Resolve(Manager, id);
                    enemy.Move(xloc, yloc);
                    EnterWorld(enemy);
                }

                if (Wave < 10)
                    SpawnBoss(_bosses);
                else if (Wave >= 10 && Wave < 25)
                    SpawnBoss(_bosses10);
                else
                    SpawnBoss(_bosses20);

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