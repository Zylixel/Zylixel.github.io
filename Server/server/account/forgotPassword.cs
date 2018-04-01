#region

using db;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

#endregion

namespace server.account
{
    internal class forgotPassword : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                string authKey = Database.GenerateRandomString(128);
                var cmd = db.CreateQuery();
                cmd.CommandText = "UPDATE accounts SET authToken=@auth WHERE uuid=@email;";
                cmd.Parameters.AddWithValue("@auth", authKey);
                cmd.Parameters.AddWithValue("@email", Query["guid"]);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    var username = "";
                    cmd = db.CreateQuery();
                    cmd.CommandText = "SELECT name from accounts Where uuid=@email;";
                    cmd.Parameters.AddWithValue("@email", Query["guid"]);
                    using (var rdr = cmd.ExecuteReader())
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            username = rdr.GetString("name");
                            rdr.Close();
                        }

                    MailMessage message = new MailMessage();
                    message.To.Add(Query["guid"]);
                    message.Subject = "New Password Requested";
                    message.From = new MailAddress(Program.Settings.GetValue<string>("serverEmail", ""), "Frostz' Realm Staff");
                    message.Body = emailBody.Replace("{PASSWORDLINK}", Program.Settings.GetValue<string>("serverDomain", "Error") + "/account/resetPassword?authtoken=" + authKey)
                                            .Replace("{USERNAME}", username);
                   
                    Program.SendEmail(message, true);
                }
                else
                    using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                        wtr.Write("<Error>Error.accountNotFound</Error>");
            }
        }

        const string emailBody = @"Hello {USERNAME},

Your password for Frostz' Realm was requested to be reset. 

Get your new password here:
{PASSWORDLINK}

If you did not request this, ignore this email.
Do not reply to this email, it will not be read.

- Frostz' Realm Staff";
    }
}
//If you did not do this please contact a staff member here: https://discord.gg/HtdwsAc