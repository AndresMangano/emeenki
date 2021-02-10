namespace Hermes.Worker
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string QueriesHub { get; set; }
        public QueueSettings Queue { get; set; }
    }

    public class QueueSettings
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}