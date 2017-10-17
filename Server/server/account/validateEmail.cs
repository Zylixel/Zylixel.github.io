using System.IO;
using db;

namespace server.account
{
    internal class ValidateEmail : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "UPDATE accounts SET verified=1 WHERE authToken=@authToken";
                cmd.Parameters.AddWithValue("@authToken", Query["authToken"]);
                using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                {
                    if (cmd.ExecuteNonQuery() == 1)
                        Program.SendFile("game/verifySuccess.html", Context);
                    else
                        Program.SendFile("game/verifyFail.html", Context);
                }
            }
        }
    }
}
