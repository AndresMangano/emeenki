using System;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Core.Repositories
{
    public interface IArticleTemplateRepository
    {
        void InsertArticleTemplate(Guid articleTemplateID, bool deleted, string languageID, string source, string photoURL, DateTime timestamp);
        void UpdateArticleTemplate(Guid articleTemplateID,
            DbUpdate<bool> deleted = null);
    }
}