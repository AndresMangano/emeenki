using System;
using System.Threading.Tasks;
using Hermes.Worker.Core.Ports;
using Microsoft.AspNetCore.SignalR.Client;

namespace Hermes.Worker.Shell
{
    public partial class Interpreter : ISignalRPort
    {
        HubConnection _connection;

        void InitSignalR(string hubUrl)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();
            _connection.StartAsync();
        }

        public async Task SendSignalToGroup(string signal, string message, params string[] groups)
        {
            foreach (var group in groups) {
                await _connection.InvokeAsync("SendMessageToGroup", group, signal, message);
            }
        }
    }
}