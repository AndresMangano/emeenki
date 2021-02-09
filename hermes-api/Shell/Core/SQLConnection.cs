using System;
using System.Reflection;
using System.Threading;
using DbUp;

namespace Hermes.Shell
{
    public sealed class SQLConnection
    {
        public string ConnectionString { get; }

        public SQLConnection(string connectionString)
        {
            ConnectionString = connectionString;

            for (var retries=10; retries > 0; ) {
                try {
                    var upgraderResult =
                        DeployChanges
                            .To
                            .MySqlDatabase(connectionString)
                            .WithScriptsAndCodeEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                            .LogToConsole()
                            .Build()
                            .PerformUpgrade();
                    break;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    retries--;
                    Thread.Sleep(5000);
                }
            }
        }

        public void DestroyDB()
        {
            DbUp.DropDatabase.For.SqlDatabase(ConnectionString);
        }
    }
}
