#region

using System.Collections.Generic;
using wServer.networking;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.worlds
{
    public class Test : World
    {
        public string Js;

        public Test()
        {
            Id = TEST_ID;
            Name = "Test";
            Background = 0;
        }

        public void LoadJson(string json)
        {
            Js = json;
            LoadMap(json);
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);

            foreach (KeyValuePair<int, Player> i in Players)
            {
                if (i.Value.Client.Account.Rank < 2)
                {
                    i.Value.Client.Disconnect(Client.DisconnectReason.INVALID_PORTAL_KEY);
                }
            }
        }

        protected override void Init() { }
    }
}