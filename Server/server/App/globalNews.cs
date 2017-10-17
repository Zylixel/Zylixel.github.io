#region

using System.Collections.Generic;
using System.Text;
using db;
using Newtonsoft.Json;

#endregion

namespace server.app
{
    internal class GlobalNews : RequestHandler
    {
        protected override void HandleRequest()
        {
            string s = "[";
            
            using (Database db = new Database())
            {
                var toSerialize = GetGlobalNews(db);
                int len = toSerialize.Count;
            
                for (int i = 0; i < len; i++)
                {
                    if (toSerialize.Count > 1)
                        s += JsonConvert.SerializeObject(toSerialize[0]) + ",";
                    else
                        s += JsonConvert.SerializeObject(toSerialize[0]);
                    toSerialize.RemoveAt(0);
                }
                s += "]";
            }

            byte[] buf = Encoding.UTF8.GetBytes(s);
            Context.Response.OutputStream.Write(buf, 0, buf.Length);
        }

        private List<GlobalNewsStruct> GetGlobalNews(Database db)
        {
            List<GlobalNewsStruct> ret = new List<GlobalNewsStruct>();
            var cmd = db.CreateQuery();
            cmd.CommandText = "SELECT * FROM globalNews WHERE endTime >= now();";
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    ret.Add(new GlobalNewsStruct
                    {
                        Slot = rdr.GetInt32("slot"),
                        LinkType = rdr.GetInt32("linkType"),
                        Title = rdr.GetString("title"),
                        Image = rdr.GetString("image"),
                        Priority = rdr.GetInt32("priority"),
                        LinkDetail = rdr.GetString("linkDetail"),
                        Platform = rdr.GetString("platform"),
                        StartTime = long.Parse(Database.DateTimeToUnixTimestamp(rdr.GetDateTime("startTime")) + "000"),
                        EndTime = long.Parse(Database.DateTimeToUnixTimestamp(rdr.GetDateTime("endTime")) + "000")
                    });
                }
            }

            return ret;
        }
    }

    public struct GlobalNewsStruct
    {
        public int Slot;
        public int LinkType;
        public string Title;
        public string Image;
        public int Priority;
        public string LinkDetail;
        public string Platform;
        public long StartTime;
        public long EndTime;
    }
}