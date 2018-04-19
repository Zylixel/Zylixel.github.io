#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;
using db;
using MySql.Data.MySqlClient;
using server.package;
using Newtonsoft.Json;
using System.Linq;

#endregion

namespace server.account
{
    public class purchasePackage : RequestHandler
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

                    Account acc = db.Verify(Query["guid"], Query["password"], Program.GameData);

                    if (CheckAccount(acc, db, false))
                    {
                        if (acc.Credits < package.Price)
                        {
                            wtr.Write("<Error>Not enough gold.<Error/>");
                            return;
                        }

                        List<int> claimed = Utils.FromCommaSepString32(package.usersClaimed).ToList();
                        if (package.MaxPurchase == 1)
                        {
                            foreach (int accId in claimed)
                                if (accId == Convert.ToInt32(acc.AccountId))
                                {
                                    using (StreamWriter wtr2 = new StreamWriter(Context.Response.OutputStream))
                                        wtr2.WriteLine("<Error>Package already claimed!</Error>");
                                    return;
                                }
                        }

                        claimed.Add(Convert.ToInt32(acc.AccountId));
                        var cmd = db.CreateQuery();
                        cmd.CommandText =
                            "UPDATE packages SET usersClaimed=@usersClaimed WHERE name=@packageName;";
                        cmd.Parameters.AddWithValue("@usersClaimed", Utils.GetCommaSepString(claimed.ToArray()));
                        cmd.Parameters.AddWithValue("@packageName", package.Name);
                        cmd.ExecuteNonQuery();
                        
                        List<int> gifts = acc.Gifts;
                        List<int> contents = Utils.FromCommaSepString32(package.Contents).ToList();
                        foreach (int item in contents)
                        {
                            gifts.Add(db.CreateSerial(Program.GameData.Items[(ushort)item], true, DroppedIn: "Package").serialId);
                        }

                        cmd = db.CreateQuery();
                        cmd.CommandText =
                            "UPDATE accounts SET gifts=@gifts WHERE uuid=@uuid AND password=SHA1(@password);";
                        cmd.Parameters.AddWithValue("@gifts", Utils.GetCommaSepString(gifts.ToArray()));
                        cmd.Parameters.AddWithValue("@uuid", Query["guid"]);
                        cmd.Parameters.AddWithValue("@password", Query["password"]);
                        cmd.ExecuteNonQuery();

                        db.UpdateCredit(acc, -package.Price);
                        wtr.Write("<Success/>");
                    }
                }
            }
        }
    }
}