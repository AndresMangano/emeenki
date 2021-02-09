using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Core.Repositories
{
    public interface IArticleRepository
    {
        void InsertArticle(Guid articleID, Guid articleTemplateID, bool archived, string roomID, string originalLanguageID, string translationLanguageID,
            string source, string photoURL, DateTime timestamp);
        void UpdateArticle(Guid articleID,
            DbUpdate<bool> archived = null);
    }
}