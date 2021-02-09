using System;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Core.Repositories
{
    public interface IArticleTemplatesRepository
    {
        void InsertArticleTemplates(Guid articleTemplateID, string title, DateTime created, string languageID, string photoURL, bool archived);
        void UpdateArticleTemplates(Guid articleTemplateID,
            DbUpdate<bool> archived = null);
    }
}