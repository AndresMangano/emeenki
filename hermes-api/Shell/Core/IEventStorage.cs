using System.Collections.Generic;
using Hermes.Core;

namespace Hermes.Shell
{
    public interface IEventStorage<TKey>
    {
        IEnumerable<DomainEvent<TKey, object>> GetOrderedEvents(TKey id);
        IEnumerable<EventMessage<TKey, object>> GetMissingEvents(long lastIndex);
        EventMessage<TKey, object> StoreEvent(DomainEvent<TKey, object> evnt);
    }
}