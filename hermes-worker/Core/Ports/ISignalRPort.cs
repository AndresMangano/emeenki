using System.Threading.Tasks;

namespace Hermes.Worker.Core.Ports
{
    public static class SignalRSignal
    {
        public const string ARTICLE_UPDATED = "article-updated";
        public const string ARTICLE_TEMPLATE_UPDATED = "article-template-updated";
        public const string ROOM_UPDATED = "room-updated";
        public const string USER_UPDATED = "user-updated";
    }
    public interface ISignalRPort
    {
        Task SendSignalToGroup(string signal, string message, params string[] groups);
    }
}