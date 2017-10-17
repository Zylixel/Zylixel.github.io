#region

using System.Collections.Generic;
using System.IO;
using db;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using server.package;

#endregion

namespace server.account
{
    public class PurchasePackage : RequestHandler
    {
        protected override void HandleRequest()
        {
            StreamWriter wtr = new StreamWriter(Context.Response.OutputStream);
            if (Query.AllKeys.Length > 0)
            {
                using (Database db = new Database())
                {
                    Package package = Package.GetPackage(int.Parse(Query["packageId"]));

                    if (package == null)
                    {
                        wtr.Write("<Error>This package is not available any more</Error>");
                        return;
                    }

                    JsonSerializer s = new JsonSerializer();
                    var contents = s.Deserialize<PackageContent>(new JsonTextReader(new StringReader(package.Contents)));

                    Account acc = db.Verify(Query["guid"], Query["password"], Program.GameData);

                    if (CheckAccount(acc, db, false))
                    {
                        if (acc.Credits < package.Price)
                        {
                            wtr.Write("<Error>Not enough gold.<Error/>");
                            return;
                        }

                        MySqlCommand cmd;

                        if (contents.Items?.Count > 0)
                        {
                            foreach (var i in contents.Items)
                            {
                                List<int> gifts = acc.Gifts;
                                gifts.Add(i);

                                cmd = db.CreateQuery();
                                cmd.CommandText =
                                    "UPDATE accounts SET gifts=@gifts WHERE uuid=@uuid AND password=SHA1(@password);";
                                cmd.Parameters.AddWithValue("@gifts", Utils.GetCommaSepString(gifts.ToArray()));
                                cmd.Parameters.AddWithValue("@uuid", Query["guid"]);
                                cmd.Parameters.AddWithValue("@password", Query["password"]);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        if (contents.CharSlots > 0)
                        {
                            cmd = db.CreateQuery();
                            cmd.CommandText =
                                "UPDATE accounts SET maxCharSlot=maxCharSlot + @amount WHERE uuid=@uuid AND password=SHA1(@password);";
                            cmd.Parameters.AddWithValue("@amount", contents.CharSlots);
                            cmd.Parameters.AddWithValue("@uuid", Query["guid"]);
                            cmd.Parameters.AddWithValue("@password", Query["password"]);
                            if (cmd.ExecuteNonQuery() == 0)
                                return;
                        }

                        if (contents.VaultChests > 0)
                        {
                            for (int j = 0; j < contents.VaultChests; j++)
                                db.CreateChest(acc);
                        }

                        db.UpdateCredit(acc, -package.Price);
                        wtr.Write("<Success/>");
                    }
                }
            }
        }

        struct PackageContent
        {
            public List<int> Items;
            public int VaultChests;
            public int CharSlots;
        }
    }
}