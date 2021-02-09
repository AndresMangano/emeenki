using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Core.Repositories
{
    public interface IArticlesRepository
    {
        void InsertArticles(Guid articleID, string roomID, string title, DateTime created, string originalLanguageID, string translationLanguageID, string photoURL,
            bool archived);
        void UpdateArticles(Guid articleID,
            DbUpdate<bool> archived = null);
    }
}