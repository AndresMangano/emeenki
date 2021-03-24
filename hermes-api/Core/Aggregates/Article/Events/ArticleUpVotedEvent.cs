namespace Hermes.Core
{
    public record ArticleUpVotedEvent(
        bool InText,
        int SentencePos,
        int TranslationPos,
        string UserID
    );
}