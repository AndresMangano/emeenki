namespace Hermes.Core
{
    public class ArticleDownVoteRemovedEvent
    {
        public bool InText { get; }
        public int SentencePos { get; }
        public int TranslationPos { get; }
        public string UserID { get; }

        public ArticleDownVoteRemovedEvent(bool inText, int sentencePos, int translationPos, string userID)
        {
            InText = inText;
            SentencePos = sentencePos;
            TranslationPos = translationPos;
            UserID = userID;
        }
    }
}