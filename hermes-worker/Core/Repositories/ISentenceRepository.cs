using System;

namespace Hermes.Worker.Core.Repositories
{
    public interface ISentenceRepository
    {
        void InsertSentence(Guid articleID, bool inText, int sentenceIndex, string originalText);
    }
}