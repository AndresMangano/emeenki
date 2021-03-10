using System;

namespace Hermes.Core.Ports
{
    public interface IForumPostRepository
    {
        ForumPost FetchForumPost(Guid forumPostID);
    }
}