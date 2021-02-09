using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hermes.Core;
using MySql.Data.MySqlClient;

namespace Hermes.Shell.Read
{
    public class LanguageQuery : ILanguageQueries
    {
        private readonly string _connectionString;

        public LanguageQuery(SQLConnection connection)
        {
            _connectionString = connection.ConnectionString;
        }

        public async Task<IEnumerable<LanguageDTO>> List()
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)){
                conn.Open();
                return await conn.QueryAsync<LanguageDTO>(
                    "SELECT * FROM Language_Language"
                );
            }
        }
    }
}