using System.Reflection;
using DbUp;

namespace Hermes.Shell
{
    public sealed class SQLConnection
    {
        public string ConnectionString { get; }

        public SQLConnection(string connectionString)
        {
            ConnectionString = connectionString;

            var upgraderResult =
                DeployChanges
                    .To
                    .MySqlDatabase(connectionString)
                    .WithScriptsAndCodeEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build()
                    .PerformUpgrade();
        }

        public void DestroyDB()
        {
            DbUp.DropDatabase.For.SqlDatabase(ConnectionString);
        }
    }
}
