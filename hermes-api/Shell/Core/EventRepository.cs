using System;
using System.Collections.Generic;
using Hermes.Core;

namespace Hermes.Shell
{
    public sealed class EventRepository<TKey, TAggregate>
        where TAggregate : IAggregateRoot<TKey>, new()
    {
        public event Action<EventMessage<TKey, object>> OnEventStored = delegate {};
        
        private readonly IEventStorage<TKey> _eventStorage;

        public EventRepository(IEventStorage<TKey> eventStorage)
        {
            _eventStorage = eventStorage;
        }
        public TAggregate Fetch(TKey id, Action<TAggregate, DomainEvent<TKey, object>> applyFn)
        {
            TAggregate aggregate = new TAggregate();
            foreach(var e in _eventStorage.GetOrderedEvents(id)){
                applyFn(aggregate, e);
            }

            return aggregate;
        }

        public IEnumerable<EventMessage<TKey, object>> GetMissingEvents(long lastIndex)
        {
            return _eventStorage.GetMissingEvents(lastIndex);
        }

        public long? StoreEvent(DomainEvent<TKey, object> @event)
        {
            try {
                var message = _eventStorage.StoreEvent(@event);
                OnEventStored(message);
                return message.Index;
            } catch(Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Exception: ");
                Console.ResetColor();
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}