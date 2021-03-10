namespace Hermes.Core
{
    public record ArticleUpVoteRemovedEvent(
        bool InText,
        int SentencePos,
        int TranslationPos,
        string UserID
    );
}