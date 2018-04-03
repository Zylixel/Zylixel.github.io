using db;
using System;
using System.Collections.Generic;
using wServer.networking.cliPackets;

namespace wServer.realm.entities.player
{
    public partial class Player
    {
        public enum possibleExploit
        {
            MANA,
            DEXTERITY,
            GOD,
            NOCLIP,
            DIFF_WEAPON,
            BANNED_ITEM,
            DUPED,
            DIFF_ITEM,
            INAVLID_INVSWAP,
            ITEM_STEAL,
            SB_EXCHANGE,
            FAST_CLIENT
        }

        Dictionary<possibleExploit, string> cheatInfo = new Dictionary<possibleExploit, string>
            {
                { possibleExploit.MANA, "Infinite Mana" },
                { possibleExploit.DEXTERITY, "Extreme Dexterity" },
                { possibleExploit.GOD, "God" },
                { possibleExploit.NOCLIP, "No-Clip" },
                { possibleExploit.DIFF_WEAPON, "Different Weapon Shoot" },
                { possibleExploit.BANNED_ITEM, "Banned Items" },
                { possibleExploit.DUPED, "Duped Item" },
                { possibleExploit.DIFF_ITEM, "Fake Item" },
                { possibleExploit.INAVLID_INVSWAP, "Inavlid Inventory Slot" },
                { possibleExploit.ITEM_STEAL, "Steal Items" },
                { possibleExploit.SB_EXCHANGE, "Other Users Soulbound Item" },
                { possibleExploit.FAST_CLIENT, "FPS Increase" },
            };

        public bool kickforCheats(possibleExploit cheat)
        {
            SendError($"{cheatInfo[cheat]} exploit detected. ID:{(int)cheat}");
            UpdateCount++;
            Client.Save();
            Client.Disconnect("Cheats");
            Program.writeError($"Player {Client.Player.Name} used {cheatInfo[cheat]} exploit. ID: {(int)cheat}");
            foreach (var i in Manager.Clients.Values)
                if (i.Account.Rank > 1)
                    i.Player.SendError($"Player {Client.Player.Name} used {cheatInfo[cheat]} exploit. ID: {(int)cheat}");
            return false;
        }

        public void ReportforCheats(possibleExploit cheat)
        {
            SendError($"{cheatInfo[cheat]} exploit detected. ID:{(int)cheat}");
            UpdateCount++;
            Client.Save();
            Program.writeError($"Player {Client.Player.Name} used {cheatInfo[cheat]} exploit. ID:{(int)cheat}");
            foreach (var i in Manager.Clients.Values)
                if (i.Account.Rank > 1)
                    i.Player.SendError($"Player {Client.Player.Name} used {cheatInfo[cheat]} exploit. ID:{(int)cheat}");
        }

        public int lastShootTime;
        public int shootCounter;
        public int dexShotViolation;
        public int dGoodCount = 0;

        public int lastMoveTime = -1;
        public int outOfBoundsCount = 0;
        public int goodCount = 0;

        public bool CheckShootSpeed(OldItem item)
        {
            shootCounter++;
            if (lastShootTime == -1)
                lastShootTime = Environment.TickCount;

            int tolerance = 240;
            float diff = (Environment.TickCount - lastShootTime) + tolerance;
            var dt = (int)(1 / StatsManager.GetAttackFrequency() * 1 / item.RateOfFire);

            if (diff < dt)
            {
                if (shootCounter > item.NumProjectiles)
                {
                    dexShotViolation++;
                    dGoodCount = 0;
                    return false;
                }
            }
            else
            {
                shootCounter = 0;
                lastShootTime = Environment.TickCount;
                if (dexShotViolation > 0)
                {
                    dGoodCount++;
                    if (dGoodCount >= 2)
                    {
                        dexShotViolation = 0;
                        dGoodCount = 0;
                    }
                }
                else
                {
                    dGoodCount = 0;
                }
            }
            return true;
        }

        public bool isLagging;
        public int healthViolation;
        public bool forgiveHealthViolations;
        public bool detectGodExploit = false;
        private int FPScount;
        private int FPSgood;

        public bool checkforCheats(RealmTime time)
        {
            if (!(oldClientTime == 0 || first5 < 5 || Program.isLagging))
            {
                /*if (curClientTime - oldClientTime <= 2 && !isLagging)
                {
                    isLagging = true;
                    SendInfo("Lag Detected, giving Invulnerability to prevent death");
                    Program.writeNotable($"{Name} Is lagging, making invulnerable");
                }*/
                if (curClientTime - oldClientTime >= 300)
                    FPScount++;
                else
                {
                    if (FPSgood > 2)
                    {
                        FPScount = 0;
                        goodCount = 0;
                    } else
                        FPSgood++;
                } 

            }

            if (FPScount > 4)
                return kickforCheats(possibleExploit.FAST_CLIENT);

            if (isLagging)
                ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Invulnerable,
                    DurationMS = 1 * 1000
                });

            if (Mp < -5)
                return kickforCheats(possibleExploit.MANA);

            if (_buyCooldown > 0)
                _buyCooldown--;

            if (healthViolation >= 3 && !forgiveHealthViolations && detectGodExploit)
                return kickforCheats(possibleExploit.GOD);

            if (dexShotViolation >= 15)
                return kickforCheats(possibleExploit.DEXTERITY);

            if (Owner != null)
                if (!Owner.IsPassable((int)X, (int)Y) && Owner.Name != "The Other Side")
                    return kickforCheats(possibleExploit.NOCLIP);

            bool Bankick = false;
            bool Dupekick = false;
            bool SBkick = false;
            if (UpdateCount % (Manager?.TPS * 3) == 0) //Every 3 Seconds b/c it takes up alot of processing power with many players
            {
                List<int> serialList = new List<int>(); //This code handles all the serial detections and updates, put it in one place because we call it every tick and want to decrease lag
                for (var i = 0; i < Inventory.Length; i++)
                {
                    bool updateSerial = false;
                    bool delItem = false;
                    if (Inventory[i] == null) continue;
                    if (Inventory[i].firstUser == -1)
                    {
                        Inventory[i].firstUser = Convert.ToInt32(AccountId);
                        updateSerial = true;
                    }
                    if (Inventory[i].currentUser != Convert.ToInt32(AccountId))
                    {
                        Inventory[i].currentUser = Convert.ToInt32(AccountId);
                        updateSerial = true;
                    }

                    if (Inventory[i].currentUser != Inventory[i].firstUser && Inventory[i].Soulbound)
                    {
                        updateSerial = true;
                        SBkick = true;
                        delItem = true;
                    }
                    if (Inventory[i].banned == 1)
                    {
                        updateSerial = true;
                        Bankick = true;
                        delItem = true;
                    }
                    if (serialList.Contains(Inventory[i].serialId) || Inventory[i].serialId == -1)
                    {
                        updateSerial = true;
                        Dupekick = true;
                        delItem = true;
                    }
                    else
                        serialList.Add(Inventory[i].serialId);

                    if (delItem)
                        Inventory[i].banned = 1;

                    if (updateSerial)
                        using (Database db = new Database())
                            db.UpdateSerial(Inventory[i]);

                    if (delItem)
                        Inventory[i] = null;
                }

                if (Dupekick)
                    kickforCheats(possibleExploit.DUPED);
                if (Bankick)
                    ReportforCheats(possibleExploit.BANNED_ITEM);
                if (SBkick)
                    ReportforCheats(possibleExploit.SB_EXCHANGE);
            }

            return (!SBkick && !Dupekick && !Bankick);
        }
    }
}
