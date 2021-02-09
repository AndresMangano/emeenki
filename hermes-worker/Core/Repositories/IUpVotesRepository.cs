using System;

namespace Hermes.Worker.Core.Repositories
{
    public interface IUpVotesRepository
    {
        void InsertUpVote(Guid articleID, bool inText, int sentenceIndex, int translationIndex, string userID);
        void DeleteUpVote(Guid articleID, bool inText, int sentenceIndex, int translationIndex, string userID);
    }
}