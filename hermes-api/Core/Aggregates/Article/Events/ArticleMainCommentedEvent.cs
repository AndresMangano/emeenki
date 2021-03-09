namespace Hermes.Core
{
    public record ArticleMainCommentedEvent(
        int CommentPos,
        int? ChildCommentPos,
        string Comment,
        string UserID
    );
}