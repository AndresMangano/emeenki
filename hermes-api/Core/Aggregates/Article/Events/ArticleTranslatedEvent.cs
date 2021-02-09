namespace Hermes.Core
{
    public class ArticleTranslatedEvent
    {
        public bool InText { get; }
        public int SentencePos { get; }
        public int TranslationPos { get; }
        public string Translation { get; }
        public string UserID { get; }

        public ArticleTranslatedEvent(bool inText, int sentencePos, int translationPos, string translation, string userID)
        {
            InText = inText;
            SentencePos = sentencePos;
            TranslationPos = translationPos;
            Translation = translation;
            UserID = userID;
        }
    }
}