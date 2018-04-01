using db;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace server.account
{
    internal class resetPassword : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                var password = Database.GenerateRandomString(10);
                var cmd = db.CreateQuery();
                cmd.CommandText = "UPDATE accounts SET password=SHA1(@password) WHERE authToken=@authToken;";
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@authToken", Query["authToken"]);
                bool success = cmd.ExecuteNonQuery() > 0;

                var username = "";
                var guid = "";
                cmd = db.CreateQuery();
                cmd.CommandText = "SELECT * from accounts Where authToken=@authToken;";
                cmd.Parameters.AddWithValue("@authToken", Query["authToken"]);
                using (var rdr = cmd.ExecuteReader())
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        username = rdr.GetString("name");
                        guid = rdr.GetString("uuid");
                        rdr.Close();
                    }

                using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                {
                    if (success)
                    {
                        wtr.Write(resetPasswordSuccess);

                        MailMessage message = new MailMessage();
                        message.To.Add(guid);
                        message.Subject = "Password Reset";
                        message.From = new MailAddress(Program.Settings.GetValue<string>("serverEmail", ""), "Frostz' Realm Staff");
                        message.Body = emailBody.Replace("{PASSWORD}", password).Replace("{USERNAME}", username);
                        Program.SendEmail(message, true);
                    }
                    else
                        wtr.Write(resetPasswordFailure);
                }
            }
        }

        private const string resetPasswordSuccess =
@"<html>
<body bgcolor=""#000000"">
    <div align=""center"">
        <font size=""10"" color=""#FFFFFF"">Account Verified. We sent a new passowrd to your e-mail.</font>
    </div>
</body>
</html>";

        private const string resetPasswordFailure =
@"<html>
<body bgcolor=""#000000"">
    <div align=""center"">
        <font size=""10"" color=""#FFFFFF"">Ohhh something went wrong, please request a new password.</font>
    </div>
</body>
</html>";

        const string emailBody = @"Hello {USERNAME},

Your password for Frostz' Realm was reset.

Your new password is:
{PASSWORD}

You can set the password to anything you'd like ingame by logging in, clicking account, and follow the 'Change password' prompt

If you did not do this please contact a staff member here: https://discord.gg/HtdwsAc
Do not reply to this email, it will not be read.

- Frostz' Realm Staff";
    }
}
