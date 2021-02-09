using System;

namespace Hermes.Core
{
    public class ArticleTemplateUploadResult
    {
        public Guid ArticleTemplateID { get; }
        public ArticleTemplateUploadResult(Guid articleTemplateID)
        {
            ArticleTemplateID = articleTemplateID;
        }
    }
}