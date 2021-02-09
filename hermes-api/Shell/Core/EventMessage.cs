using Hermes.Core;

namespace Hermes.Shell
{
    public sealed class EventMessage<TKey, TPayload>
    {
        public long Index { get; }
        public DomainEvent<TKey, TPayload> Message { get; }

        public EventMessage(long index, DomainEvent<TKey, TPayload> message)
        {
            Index = index;
            Message = message;
        }
    }
}