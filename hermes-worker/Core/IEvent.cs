using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core
{
    public interface IEvent<TKey>
    {
        EventHeader<TKey> Header { get; }
        
        void Apply(DBInterpreter interpreter);
        void Notify(ISignalRPort signalR);
    }
}