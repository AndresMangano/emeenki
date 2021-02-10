using System;
using Newtonsoft.Json;

namespace Hermes.Worker.Shell
{
    public partial class Interpreter
    {
        readonly AppSettings _settings;

        public Interpreter(AppSettings settings) {
            _settings = settings;
            InitSignalR(_settings.QueriesHub);
        }
    }
}