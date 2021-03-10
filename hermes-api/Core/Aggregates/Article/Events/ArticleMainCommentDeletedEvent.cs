namespace Hermes.Core
{
    public record ArticleMainCommentDeletedEvent(
        int CommentPos,
        int? ChildCommentPos
    );
}