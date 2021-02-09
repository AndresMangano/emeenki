using System;

namespace Hermes.Core
{
    public class ArticleTakeTemplateResult
    {
        public Guid ArticleID { get; }
        public ArticleTakeTemplateResult(Guid articleID)
        {
            ArticleID = articleID;
        }
    }
}