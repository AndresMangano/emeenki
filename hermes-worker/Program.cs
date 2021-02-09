using System.IO;
using Hermes.Worker.Core.Commands;
using Hermes.Worker.Shell;
using Microsoft.Extensions.Configuration;

namespace Hermes.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var appSettings = configuration
                .Get<AppSettings>();
            
            Interpreter io = new Interpreter(appSettings);
            SetupQueuesCommand.Execute<Interpreter, DBInterpreter>(io);
        }
    }
}
