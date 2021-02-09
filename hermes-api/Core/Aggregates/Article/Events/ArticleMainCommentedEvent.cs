namespace Hermes.Core
{
    public class ArticleMainCommentedEvent
    {
        public int CommentPos { get; }
        public int? ChildCommentPos { get; }
        public string Comment { get; }
        public string UserID { get; }

        public ArticleMainCommentedEvent(int commentPos, int? childCommentPos, string comment, string userID) {
            CommentPos = commentPos;
            ChildCommentPos = childCommentPos;
            Comment = comment;
            UserID = userID;
        }
    }
}