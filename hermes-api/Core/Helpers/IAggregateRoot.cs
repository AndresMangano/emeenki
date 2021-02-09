namespace Hermes.Core
{
    public interface IAggregateRoot<TKey>
    {   
        TKey ID { get; }
        int Version { get; }
        bool Created { get; }
        bool Deleted { get; }
    }
}