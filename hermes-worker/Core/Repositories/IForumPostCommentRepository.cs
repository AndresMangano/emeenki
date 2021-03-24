using System;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Core.Repositories
{
    public interface IForumPostCommentRepository
    {
        void InsertForumPostComment(Guid ID, Guid forumPostID, string text, string userID, DateTime timestamp);
        void DeleteForumPostComment(Guid ID);
    }
}