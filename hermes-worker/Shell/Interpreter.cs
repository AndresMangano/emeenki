using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.Worker.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hermes.Worker.Shell
{
    public partial class Interpreter
    {
        readonly AppSettings _settings;
        readonly EventProcessor _handler;
        readonly ILogger<Interpreter> _logger;
        readonly ILoggerFactory _loggerFactory;

        public Interpreter(AppSettings settings, ILoggerFactory loggerFactory) {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<Interpreter>();
            _settings = settings;
            InitSignalR(_settings.QueriesHub);
            _handler = new EventProcessor(settings.ConnectionString, this, this, this, _loggerFactory);
        }

        public async Task Start()
        {
            await _handler.Start();
        }
    }
}