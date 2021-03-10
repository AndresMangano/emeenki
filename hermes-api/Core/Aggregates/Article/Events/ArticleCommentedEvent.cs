namespace Hermes.Core
{
    public record ArticleCommentedEvent(
        bool InText,
        int SentencePos,
        int TranslationPos,
        int CommentPos,
        string Comment,
        string UserID
    );
}