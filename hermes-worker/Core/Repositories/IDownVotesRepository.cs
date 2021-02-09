using System;

namespace Hermes.Worker.Core.Repositories
{
    public interface IDownVotesRepository
    {
        void InsertDownVote(Guid articleID, bool inText, int sentenceIndex, int translationIndex, string userID);
        void DeleteDownVote(Guid articleID, bool inText, int sentenceIndex, int translationIndex, string userID);
    }
}