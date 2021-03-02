using System;
using Dapper;
using Hermes.Worker.Core.Model;
using Hermes.Worker.Core.Ports;
using MySql.Data.MySqlClient;

namespace Hermes.Worker.Shell
{
    public partial class Interpreter : IUnitOfWorkPort<DBInterpreter>
    {
        public void Transaction<K, V>(DefaultMessage<K, V> message, Action<DBInterpreter> unitOfWork)
        {
            using(MySqlConnection conn = new MySqlConnection(_settings.ConnectionString)) {
                conn.Open();
                using(MySqlTransaction tran = conn.BeginTransaction()) {
                    try {
                        unitOfWork(new DBInterpreter(conn, tran));
                        conn.Execute(@"
                            INSERT INTO Worker_Handlers(Stream, SeqID)
                            VALUES (@stream, @secId)
                                ON DUPLICATE KEY UPDATE SeqID = @secId", new {
                                stream = message.Message.Metadata.Stream,
                                seqId = message.Index
                            });
                        tran.Commit();
                    } catch (Exception) {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}