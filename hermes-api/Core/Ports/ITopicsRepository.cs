namespace Hermes.Core.Ports
{
    public interface ITopicsRepository
    {
        Topic FetchTopic(string topicID);
    }
}