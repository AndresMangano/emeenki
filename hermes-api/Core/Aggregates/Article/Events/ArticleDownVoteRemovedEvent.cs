namespace Hermes.Core
{
    public record ArticleDownVoteRemovedEvent(
        bool InText,
        int SentencePos,
        int TranslationPos,
        string UserID
    );
}