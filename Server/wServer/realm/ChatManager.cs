using db;
using wServer.networking;
using wServer.networking.svrPackets;
using wServer.realm.entities.player;
using System;

namespace wServer.realm
{
    public class ChatManager
    {
        RealmManager manager;
        

        public ChatManager(RealmManager manager)
        {
            this.manager = manager;
        }

        public void Say(Player src, string text)
        {
            string prefix = "";
            switch (src.Client.Account.Rank)
            {
                case 1: prefix = "!"; break;
                case 2: prefix = "@"; break;
                case 3: prefix = "#"; break;
                case 4: prefix = "$"; break;
            }

            src.Owner.BroadcastPacketSync(new TextPacket
            {
                Name = prefix + src.Name,
                ObjectId = src.Id,
                Stars = src.Stars,
                BubbleTime = 10,
                Recipient = "",
                Text = text.ToSafeText(),
                CleanText = text.ToSafeText()
            }, p => !p.Ignored.Contains(src.AccountId));
            Console.WriteLine("[{0}({1})] <{2}> {3}", src.Owner.Name, src.Owner.Id, src.Name, text);
            src.Owner.ChatReceived(text);
        }

        public void SayGuild(Player src, string text)
        {
            foreach (Client i in src.Manager.Clients.Values)
            {
                if (Equals(src.Guild, i.Player.Guild))
                {
                    i.SendPacket(new TextPacket
                    {
                        Name = src.ResolveGuildChatName(),
                        ObjectId = src.Id,
                        Stars = src.Stars,
                        BubbleTime = 10,
                        Recipient = "*Guild*",
                        Text = text.ToSafeText()
                    });
                }
            }
        }

        public void News(string text)
        {
            foreach (var i in manager.Clients.Values)
                i.SendPacket(new TextPacket
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "#Zylixel News",
                    Text = text.ToSafeText()
                });
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("<Zylixel News> {0}", text);
        }

        public void Announce(string text)
        {
            foreach (var i in manager.Clients.Values)
                i.SendPacket(new TextPacket
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "@Announcement",
                    Text = text.ToSafeText()
                });
                Console.WriteLine("<Announcement> {0}", text);
        }
        
        public void Oryx(World world, string text)
        {
            world.BroadcastPacket(new TextPacket
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "#Oryx the Mad God",
                Text = text.ToSafeText()
            }, null);
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("[{0}({1})] <Oryx the Mad God> {2}", world.Name, world.Id, text);
        }
    }
}
