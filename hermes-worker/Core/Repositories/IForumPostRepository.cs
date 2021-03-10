using System;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Core.Repositories
{
    public interface IForumPostRepository
    {
        void InsertForumPost(Guid ID, string title, string text, string languageID, string userID, DateTime timestamp);
        void UpdateForumPost(Guid ID,
            DbUpdate<string> title = null,
            DbUpdate<string> text = null,
            DbUpdate<string> languageID = null,
            DbUpdate<DateTime> modifiedOn = null);
        void DeleteForumPost(Guid ID);
    }
}