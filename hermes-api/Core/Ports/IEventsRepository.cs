namespace Hermes.Core.Ports
{
    public interface IEventsRepository
    {
        void StoreEvent(IDomainEvent evnt);
    }
}