using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core
{
    public interface IEvent
    {
        EventHeader Header { get; }
        
        void Apply(DBInterpreter interpreter);
        void Notify(ISignalRPort signalR);
    }
}