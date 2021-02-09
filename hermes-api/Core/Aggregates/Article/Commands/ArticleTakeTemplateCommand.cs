using System;

namespace Hermes.Core
{
    public class ArticleTakeTemplateCommand
    {
        public Guid ArticleTemplateID { get; set; }
        public string RoomID { get; set; }
    }
}