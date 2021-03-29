using System;
using System.Threading.Tasks;

namespace Hermes.Worker.Core.Ports
{
    public static class SignalRSignal
    {
        public const string ARTICLE_UPDATED = "article-updated";
        public const string ARTICLE_TEMPLATE_UPDATED = "article-template-updated";
        public const string ROOM_UPDATED = "room-updated";
        public const string USER_UPDATED = "user-updated";
        public const string FORUM_POST_UPDATED = "forum-post-updated";
    }

    public static class SignalRGroup
    {
        public const string ARTICLES = "articles";
        public static string Article(Guid ID) => $"article:{ID}";
        public const string ARTICLE_TEMPLATES = "articleTemplates";
        public const string ROOMS = "rooms";
        public static string Room(string ID) => $"room:{ID}";
        public const string USERS = "users";
        public static string User(string ID) => $"user:{ID}";
        public const string FORUM_POSTS = "forumPosts";
        public static string ForumPost(Guid ID) => $"forumPost:{ID}";
    }

    public interface ISignalRPort
    {
        Task SendSignalToGroup(string signal, string message, params string[] groups);
    }
}