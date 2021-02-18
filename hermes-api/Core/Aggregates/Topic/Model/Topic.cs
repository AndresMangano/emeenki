namespace Hermes.Core
{
    public class Topic : IAggregateRoot<string>
    {
        public string ID { get; set; }
        public int Version { get; set; }
        public bool Created { get; set; }
        public bool Deleted { get; set; }
        public string Name { get; set; }
    }
}