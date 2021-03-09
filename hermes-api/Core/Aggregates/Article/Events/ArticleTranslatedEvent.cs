namespace Hermes.Core
{
    public record ArticleTranslatedEvent(
        bool InText,
        int SentencePos,
        int TranslationPos,
        string Translation,
        string UserID
    );
}