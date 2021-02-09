using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Core.Repositories
{
    public interface IArticleCommentsRepository
    {
        void InsertArticleComment(Guid articleID, int commentIndex, int? childCommentIndex, string comment, string userID, DateTime timestamp);
        void UpdateArticleComment(Guid articleID, int commentIndex, DbUpdate<int?> childCommentIndex,
            DbUpdate<bool> deleted = null);
    }
}