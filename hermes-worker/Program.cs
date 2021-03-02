using System.Diagnostics;
using System.IO;
using Hermes.Worker.Shell;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Hermes.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
                builder.AddSimpleConsole(options =>
                {
                    options.ColorBehavior = LoggerColorBehavior.Enabled;
                    options.TimestampFormat = "hh:mm:ss";
                }));
                
            var logger = loggerFactory.CreateLogger<Program>();
            
            logger.LogInformation("Load settings");
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            logger.LogInformation("Parse settings");
            var appSettings = configuration
                .Get<AppSettings>();
            
            Interpreter interpreter = new Interpreter(appSettings, loggerFactory);
            interpreter.Start();
        }
    }
}
