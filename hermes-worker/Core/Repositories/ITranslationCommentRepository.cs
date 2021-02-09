using System;

namespace Hermes.Worker.Core.Repositories
{
    public interface ITranslationCommentRepository
    {
        void InsertTranslationComment(Guid articleID, bool inText, int sentenceIndex, int translationIndex, int commentIndex, string comment, string userID, DateTime timestamp);
    }
}