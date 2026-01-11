using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Core.Repositories
{
    public interface IArticlesRepository
    {
        /// <summary>
        /// Original signature, kept for existing callers.
        /// </summary>
        void InsertArticles(
            Guid articleID,
            string roomID,
            string title,
            DateTime created,
            string originalLanguageID,
            string translationLanguageID,
            string photoURL,
            bool archived);

        /// <summary>
        /// New overload that supports video metadata + template reference.
        /// </summary>
        void InsertArticles(
            Guid articleID,
            string roomID,
            string title,
            DateTime created,
            string originalLanguageID,
            string translationLanguageID,
            string photoURL,
            bool archived,
            bool isVideo,
            string videoURL,
            Guid? articleTemplateID);

        void UpdateArticles(
            Guid articleID,
            DbUpdate<bool> archived = null);
    }
}