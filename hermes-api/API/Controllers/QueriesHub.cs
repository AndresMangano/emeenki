using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Hermes.API.Controllers
{
    public class QueriesHub : Hub
    {
        public async Task JoinGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }
        public async Task LeaveGroup(string group)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }
        public async Task SendSignalToGroup(string group, string signal)
        {
            await Clients.Group(group).SendAsync(signal);
        }
        public async Task SendMessageToGroup(string group, string signal, string message)
        {
            await Clients.Group(group).SendAsync(signal, message);
        }
    }
}