using System;

namespace Hermes.Core
{
    public class ArticleCommentMainCommand
    {
        public Guid ArticleID { get; set; }
        public string Comment { get; set; }
        public int? ParentCommentPos { get; set; }
    }
}