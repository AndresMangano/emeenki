using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hermes.Core;
using MySql.Data.MySqlClient;

namespace Hermes.Shell.Read
{
    public class TopicQuery : ITopicQueries
    {
        private readonly string _connectionString;

        public TopicQuery(SQLConnection connection)
        {
            _connectionString = connection.ConnectionString;
        }
        
        public async Task<IEnumerable<TopicDTO>> List()
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)){
                conn.Open();
                return await conn.QueryAsync<TopicDTO>(
                    "SELECT * FROM Topics"
                );
            }
        }
    }
}