namespace Hermes.Core
{
    public record ArticleDownVotedEvent(
        bool InText,
        int SentencePos,
        int TranslationPos,
        string UserID
    );
}