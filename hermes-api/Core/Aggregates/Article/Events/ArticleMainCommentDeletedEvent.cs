namespace Hermes.Core
{
    public class ArticleMainCommentDeletedEvent
    {
        public int CommentPos { get; }
        public int? ChildCommentPos { get; }

        public ArticleMainCommentDeletedEvent(int commentPos, int? childCommentPos)
        {
            CommentPos = commentPos;
            ChildCommentPos = childCommentPos;
        }
    }
}