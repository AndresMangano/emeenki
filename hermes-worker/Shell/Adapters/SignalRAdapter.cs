using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Hermes.Worker.Core.Ports;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Hermes.Worker.Shell
{
    public partial class Interpreter : ISignalRPort
    {
        HubConnection _connection;

        void InitSignalR(string hubUrl)
        {
            _logger.LogInformation("Setup SignalR connection");
            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();
            _connection.StartAsync();
        }

        public async Task SendSignalToGroup(string signal, string message, params string[] groups)
        {
            _logger.LogInformation("Send signal to groups");
            foreach (var group in groups) {
                await _connection.InvokeAsync("SendMessageToGroup", group, signal, message);
            }
        }
    }
}