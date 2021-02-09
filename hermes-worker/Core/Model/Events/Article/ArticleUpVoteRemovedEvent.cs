namespace Hermes.Worker.Core.Model.Events.Article
{
    public class ArticleUpVoteRemovedEvent
    {
        public bool InText { get; }
        public int SentencePos { get; }
        public int TranslationPos { get; }
        public string UserID { get; }

        public ArticleUpVoteRemovedEvent(bool inText, int sentencePos, int translationPos, string userID)
        {
            InText = inText;
            SentencePos = sentencePos;
            TranslationPos = translationPos;
            UserID = userID;
        }
    }
}