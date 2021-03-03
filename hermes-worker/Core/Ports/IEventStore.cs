using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hermes.Worker.Core.Ports
{
    public interface IEventStore
    {
        Task<IEnumerable<EventDTO>> GetMissingEvents(string stream, long index);
    }

    public record EventDTO(
        string RoutingKey,
        string Payload
    );
}