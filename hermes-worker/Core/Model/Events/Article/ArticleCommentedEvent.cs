namespace Hermes.Worker.Core.Model.Events.Article
{
    public class ArticleCommentedEvent
    {
        public bool InText { get; }
        public int SentencePos { get; }
        public int TranslationPos { get; }
        public int CommentPos { get; }
        public string Comment { get; }
        public string UserID { get; }

        public ArticleCommentedEvent(bool inText, int sentencePos, int translationPos, int commentPos, string comment, string userID)
        {
            InText = inText;
            SentencePos = sentencePos;
            TranslationPos = translationPos;
            CommentPos = commentPos;
            Comment = comment;
            UserID = userID;
        }
    }
}