#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using db;
using MySql.Data.MySqlClient;
using System.Net.Sockets;
using GoogleMaps.LocationServices;
using Newtonsoft.Json;
using db.data;

#endregion

namespace server.@char
{
    internal class list : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                Account a = db.Verify(Query["guid"], Query["password"], Program.GameData);
                if (!CheckAccount(a, db)) return;
                db.LockAccount(a);
                Chars chrs = new Chars
                {
                    Characters = new List<Char>(),
                    NextCharId = 2,
                    MaxNumChars = 1,
                    Account = a,
                    Servers = GetServerList()
                };
                if (chrs.Account != null)
                {
                    db.GetCharData(chrs.Account, chrs);
                    db.LoadCharacters(chrs.Account, chrs, Program.GameData);
                    chrs.News = db.GetNews(Program.GameData, chrs.Account);
                    chrs.OwnedSkins = Utils.GetCommaSepString(chrs.Account.OwnedSkins.ToArray());
                    db.UnlockAccount(chrs.Account);
                }
                else
                {
                    chrs.Account = Database.CreateGuestAccount(Query["guid"] ?? "");
                    chrs.News = db.GetNews(Program.GameData, null);
                }
                chrs.ClassAvailabilityList = GetClassAvailability(chrs.Account);

                chrs.ClassAvailabilityList = GetClassAvailability(chrs.Account);
                XmlSerializer serializer = new XmlSerializer(chrs.GetType(),
                    new XmlRootAttribute(chrs.GetType().Name) { Namespace = "" });

                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                xws.Encoding = Encoding.UTF8;
                xws.Indent = true;
                XmlWriter xtw = XmlWriter.Create(Context.Response.OutputStream, xws);
                serializer.Serialize(xtw, chrs, chrs.Namespaces);
            }
        }

        List<ServerItem> GetServerList()
        {
            var ret = new List<ServerItem>();
            int num = Program.Settings.GetValue<int>("svrNum");
            for (int i = 0; i < num; i++)
            {
                ret.Add(new ServerItem()
                {
                    Name = Program.Settings.GetValue<string>("svr" + i + "Name"),
                    DNS = Program.Settings.GetValue<string>("svr" + i + "Adr", "127.0.0.1")
                });
            }
            return ret;
        }

        private List<ClassAvailabilityItem> GetClassAvailability(Account acc)
        {
            var classes = new string[14]
            {
                "Rogue",
                "Assassin",
                "Huntress",
                "Mystic",
                "Trickster",
                "Sorcerer",
                "Ninja",
                "Archer",
                "Wizard",
                "Priest",
                "Necromancer",
                "Warrior",
                "Knight",
                "Paladin"
            };
            

            List<ClassAvailabilityItem> ret = new List<ClassAvailabilityItem>();

            using (Database db = new Database())
            {
                MySqlCommand cmd = db.CreateQuery();
                cmd.CommandText = "SELECT class, available FROM unlockedclasses WHERE accId=@accId;";
                cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                {
                    rdr.Close();
                    foreach (string s in classes)
                    {
                        MySqlCommand xcmd = db.CreateQuery();
                        xcmd.CommandText =
                            "INSERT INTO unlockedclasses(accId, class, available) VALUES(@accId, @class, @restricted);";
                        xcmd.Parameters.AddWithValue("@accId", acc.AccountId);
                        xcmd.Parameters.AddWithValue("@class", s);
                        xcmd.Parameters.AddWithValue("@restricted", "unrestricted");
                        xcmd.ExecuteNonQuery();
                        ret.Add(new ClassAvailabilityItem
                        {
                            Class = s,
                            Restricted = "unrestricted"
                        });
                    }
                }
                else
                {
                    while (rdr.Read())
                    {
                        ret.Add(new ClassAvailabilityItem
                        {
                            Class = rdr.GetString("class"),
                            Restricted = rdr.GetString("available")
                        });
                    }
                }
            }
            return ret;
        }

        public Chars GetChars(string guid, string password, XmlData data)
        {
            using (var db = new Database())
            {
                Account a = db.Verify(guid, password, data);
                if (a != null)
                {
                    if (a.Banned)
                        return null;
                }

                Chars chrs = new Chars
                {
                    Characters = new List<Char>(),
                    NextCharId = 2,
                    MaxNumChars = 1,
                    Account = a,
                };
                db.GetCharData(chrs.Account, chrs);
                db.LoadCharacters(chrs.Account, chrs, Program.GameData);
                chrs.News = db.GetNews(Program.GameData, chrs.Account);
                chrs.OwnedSkins = Utils.GetCommaSepString(chrs.Account.OwnedSkins.ToArray());
                return chrs;
            }
        }
    }
}