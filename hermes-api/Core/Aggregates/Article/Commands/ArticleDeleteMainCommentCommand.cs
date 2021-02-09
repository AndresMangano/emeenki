using System;

namespace Hermes.Core
{
    public class ArticleDeleteMainCommentCommand
    {
        public Guid ArticleID { get; set; }
        public int CommentPos { get; set; }
        public int? ChildCommentPos { get; set; }
    }
}