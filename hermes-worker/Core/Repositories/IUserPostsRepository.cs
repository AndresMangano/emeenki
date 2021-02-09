using System;

namespace Hermes.Worker.Core.Repositories
{
    public interface IUserPostsRepository
    {
        void InsertUserPost(Guid userPostID, Guid? childUserPostID, string userID, string text, string senderUserID, DateTime timestamp);
        void DeleteUserPost(Guid userPostID, Guid? childUserPostID);
    }
}