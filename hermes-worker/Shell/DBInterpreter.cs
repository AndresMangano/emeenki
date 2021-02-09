using MySql.Data.MySqlClient;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter
    {
        readonly MySqlConnection _connection;
        readonly MySqlTransaction _transaction;
        public DBInterpreter(MySqlConnection connection, MySqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }
    }
}