using System;
using Hermes.Worker.Core.Ports;
using MySql.Data.MySqlClient;

namespace Hermes.Worker.Shell
{
    public partial class Interpreter : IUnitOfWorkPort<DBInterpreter>
    {
        public void Transaction(Action<DBInterpreter> unitOfWork)
        {
            using(MySqlConnection conn = new MySqlConnection(_settings.ConnectionString)) {
                conn.Open();
                using(MySqlTransaction tran = conn.BeginTransaction()) {
                    try {
                        unitOfWork(new DBInterpreter(conn, tran));
                        tran.Commit();
                    } catch (Exception) {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Execute(Action<DBInterpreter> unitOfWork)
        {
            using(MySqlConnection conn = new MySqlConnection(_settings.ConnectionString)) {
                conn.Open();
                unitOfWork(new DBInterpreter(conn, null));
            }
        }
    }
}