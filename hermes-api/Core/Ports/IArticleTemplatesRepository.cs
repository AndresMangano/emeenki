using System;

namespace Hermes.Core.Ports
{
    public interface IArticleTemplatesRepository
    {
        ArticleTemplate FetchArticleTemplate(Guid articleTemplateID);
    }
}