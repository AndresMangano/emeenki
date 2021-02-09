using System;

namespace Hermes.Worker.Core.Repositories
{
    public interface ITranslationRepository
    {
        void InsertTranslation(Guid articleID, bool inText, int sentenceIndex, int translationIndex, string translation, string userID, DateTime timestamp);
    }
}