using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AdminPanel
{
    internal class Database
    {
        private readonly string _connStr;
        private readonly MySqlConnection _conn;

        public Database(string connStr)
        {
            _connStr = connStr;
            _conn = new MySqlConnection(_connStr);
        }

        public Task OpenAsync()
        {
            return _conn.OpenAsync();
        }

        public MySqlCommand CreateQuery(string query)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = query;
            return cmd;
        }
        public Task CreateQueryAsync(string query, params KeyValuePair<string, object>[] args)
        {
            return Task.Factory.StartNew(() =>
            {
                var cmd = CreateQuery(query);
                foreach (var parameter in args)
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                cmd.ExecuteNonQuery();
            });
        }

        public static KeyValuePair<string, object> CreateParameter(string key, object value)
        {
            return new KeyValuePair<string, object>(key, value);
        }
    }
}