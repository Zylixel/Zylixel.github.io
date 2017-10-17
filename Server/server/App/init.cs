#region

using System.IO;
using System.Text;

#endregion

namespace server.app
{
    internal class Init : RequestHandler
    {
        private readonly string _text = File.ReadAllText("init.txt");

        protected override void HandleRequest()
        {
            byte[] buf = Encoding.ASCII.GetBytes(_text);
            Context.Response.OutputStream.Write(buf, 0, buf.Length);
        }
    }
}