#region

using System;
using System.Collections.Generic;
using wServer.networking.svrPackets;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.commands
{
    internal class GuildChatCommand : Command
    {
        public GuildChatCommand() : base("guild") { }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (!player.Guild.IsDefault)
            {
                try
                {
                    var saytext = string.Join(" ", args);

                    if (string.IsNullOrWhiteSpace(saytext))
                        return Tuple.Create(false, "Usage: /guild <text>");
                    {
                        player.Guild.Chat(player, saytext.ToSafeText());
                        return Tuple.Create(true, "");
                    }
                }
                catch
                {
                    return Tuple.Create(false, "Cannot guild chat!");
                }
            }
            return Tuple.Create(false, "You need to be in a guild to use guild chat!");
        }
    }

    class GChatCommand : Command
    {
        public GChatCommand() : base("g") { }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (!player.Guild.IsDefault)
            {
                try
                {
                    var saytext = string.Join(" ", args);

                    if (string.IsNullOrWhiteSpace(saytext))
                        return Tuple.Create(false, "Usage: /guild <text>");
                    {
                        player.Guild.Chat(player, saytext.ToSafeText());
                        return Tuple.Create(true, "");
                    }
                }
                catch
                {
                    return Tuple.Create(false, "Cannot guild chat!");
                }
            }
            return Tuple.Create(false, "You need to be in a guild to use guild chat!");
        }
    }

    class GuildInviteCommand : Command
    {
        public GuildInviteCommand() : base("invite") { }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (string.IsNullOrWhiteSpace(args[0]))
                return Tuple.Create(false, "Usage: /invite <player name>");

            if (player.Guild[player.AccountId].Rank >= 20)
            {
                foreach (var unused in player.Owner.Players.Values)
                {
                    Player target = player.Owner.GetPlayerByName(args[0]);

                    if (target == null)
                    {
                        player.SendInfoWithTokens("server.invite_notfound", new KeyValuePair<string, object>("player", args[0]));
                        return Tuple.Create(false, "");
                    }
                    if (!target.NameChosen || player.Dist(target) > 20)
                    {
                        player.SendInfoWithTokens("server.invite_notfound", new KeyValuePair<string, object>("player", args[0]));
                        return Tuple.Create(false, "");
                    }

                    if (target.Guild.IsDefault)
                    {
                        target.Client.SendPacket(new InvitedToGuildPacket
                        {
                            Name = player.Name,
                            GuildName = player.Guild[player.AccountId].Name
                        });
                        target.Invited = true;
                        player.SendInfoWithTokens("server.invite_succeed", new KeyValuePair<string, object>("player", args[0]), new KeyValuePair<string, object>("guild", player.Guild[player.AccountId].Name));
                        return Tuple.Create(true, "");
                    }
                    return Tuple.Create(false, "Player is already in a guild!");
                }
            }
            else
                return Tuple.Create(false, "Members and initiates cannot invite!");
            return Tuple.Create(true, "Success");
        }
    }

    class GuildJoinCommand : Command
    {
        public GuildJoinCommand() : base("join") { }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if(string.IsNullOrWhiteSpace(args[0]))
                return Tuple.Create(false, "Usage: /join <guild name>");

            if (!player.Invited)
            {
                player.SendInfoWithTokens("server.guild_not_invited", new KeyValuePair<string, object>("guild", args[0]));
                return Tuple.Create(false, "");
            }

            player.Manager.Database.DoActionAsync(db =>
            {
                var gStruct = db.GetGuild(args[0]);
                if (player.Invited == false)
                {
                    player.SendInfo("You need to be invited to join a guild!");
                }
                if (gStruct != null)
                {
                    var g = db.ChangeGuild(player.Client.Account, gStruct.Id, 0, 0, false);
                    if (g != null)
                    {
                        player.Client.Account.Guild = g;
                        GuildManager.CurrentManagers[args[0]].JoinGuild(player);
                    }
                }
                else
                {
                    player.SendInfoWithTokens("server.guild_join_fail", new KeyValuePair<string, object>("error", "Guild does not exist"));
                }
            });
            return Tuple.Create(true, "Success");
        }
    }
}
