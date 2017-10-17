#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;
using db;
using server.account;
using server.app;
using server.@char;
using server.credits;
using server.guild;
using server.mysterybox;
using server.picture;
using server.playerMuledump;

#endregion

namespace server
{
    public abstract class RequestHandler
    {
        protected NameValueCollection Query { get; private set; }
        protected HttpListenerContext Context { get; private set; }

        public void HandleRequest(HttpListenerContext context)
        {
            Context = context;
            if (ParseQueryString())
            {
                Query = new NameValueCollection();
                using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                    Query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

                if (Query.AllKeys.Length == 0)
                {
                    string currurl = context.Request.RawUrl;
                    int iqs = currurl.IndexOf('?');
                    if (iqs >= 0)
                        Query = HttpUtility.ParseQueryString((iqs < currurl.Length - 1) ? currurl.Substring(iqs + 1) : string.Empty);
                }
            }

            HandleRequest();
        }

        public bool CheckAccount(Account acc, Database db, bool checkAccInUse=true)
        {
            if (acc == null && !String.IsNullOrWhiteSpace(Query["password"]))
            {
                WriteErrorLine("Account credentials not valid");
                return false;
            }
            if (acc == null && String.IsNullOrWhiteSpace(Query["password"]))
                return true;

            if (acc.Banned)
            {
                using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                    wtr.WriteLine("<Error>Account under maintenance</Error>");
                Context.Response.Close();
                return false;
            }
            return true;
        }

        public void WriteLine(string value, params object[] args)
        {
            using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                if (args == null || args.Length == 0) wtr.Write(value);
                else wtr.Write(value, args);
        }

        public void WriteErrorLine(string value, params object[] args)
        {
            using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                wtr.Write("<Error>" + value + "</Error>", args);
        }

        protected virtual bool ParseQueryString() => true;

        protected abstract void HandleRequest();
    }

    internal class RequestHandlers
    {
        internal static readonly Dictionary<string, RequestHandler> Handlers = new Dictionary<string, RequestHandler>
        {
     //       {"/crossdomain.xml", new Crossdomain()},
            {"/mysterybox/getBoxes", new GetBoxes()},
   //         {"/package/getPackages", new packages.getPackages()},
  //          {"/arena/getPersonalBest", new ArenaPersonalBest()},
  //          {"/arena/getRecords", new ArenaRecords()},
  //          {"/app/globalNews", new app.globalNews()},
  //          {"/app/getLanguageStrings", new app.languageSettings()},
            {"/app/init", new Init()},
  //      {"/clientError/add", new Add()},
            {"/account/purchaseSkin", new PurchaseSkin()},
            {"/app/globalNews", new Globalnews()},
            {"/account/verifyage", new Verifyage()},
            {"/account/purchasePackage", new PurchasePackage()},
            {"/playerMuledump/view", new View()},
            {"/account/acceptTOS", new AcceptTos()},
            {"/account/playFortuneGame", new PlayFortuneGame()},
            {"/account/resetPassword", new ResetPassword()},
            {"/account/validateEmail", new ValidateEmail()},
            {"/account/changeEmail", new ChangeEmail()},
            {"/account/purchaseMysteryBox", new PurchaseMysteryBox()},
            {"/account/getProdAccount", new GetProdAccount()},
            {"/account/register", new Register()},
            {"/account/verify", new Verify()},
            {"/account/forgotPassword", new ForgotPassword()},
            {"/account/sendVerifyEmail", new SendVerifyEmail()},
            {"/account/changePassword", new ChangePassword()},
            {"/account/purchaseCharSlot", new PurchaseCharSlot()},
            {"/account/setName", new SetName()},
            {"/char/list", new List()},
       //     {"/friends/getList", new friends.list()},
            {"/char/delete", new Delete()},
            {"/char/fame", new @char.Fame()},
            {"/credits/getoffers", new Getoffers()},
            {"/credits/add", new Add()},
            {"/credits/kabamadd", new Kabamadd()},
            {"/char/purchaseClassUnlock", new PurchaseClassUnlock()},
            {"/fame/list", new fame.List()},
            {"/picture/get", new Get()},
            {"/guild/getBoard", new GetBoard()},
            {"/guild/setBoard", new SetBoard()},
            {"/guild/listMembers", new ListMembers()}
        };
    }
}