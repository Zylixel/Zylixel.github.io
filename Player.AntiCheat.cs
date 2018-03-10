using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.realm.entities.player
{
    public partial class Player
    {
        private void kickforCheats(string type)
        {
            SendError($"{type} Hacks detected, If this is incorrect contact an admin");
            Client.Save();
            Client.Disconnect();
            Console.WriteLine($"Player {Client.Player.Name} is using {type} cheats");
            foreach (var i in Manager.Clients.Values)
                if (i.Account.Rank > 1)
                    i.Player.SendError($"Player {Client.Player.Name} is using {type} cheats");
        }

        public bool checkforCheats(RealmTime time)
        {
            if (Mp < 0)
            {
                kickforCheats("mana");
                return false;
            }

            if (_buyCooldown > 0)
                _buyCooldown--;

            if (checkForDex >= 3)
            {
                kickforCheats("dexterity");
                return false;
            }

            if (healthViolation >= 3)
            {
                kickforCheats("god");
                return false;
            }

            if (Owner != null)
            {
                SendUpdate(time);
                if (!Owner.IsPassable((int)X, (int)Y) && Owner.Name != "The Other Side")
                {
                    kickforCheats("no-clip");
                    return false;
                }
            }

            return true;
        }
    }
}
