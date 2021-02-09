using System;

namespace Hermes.Core.Ports
{
    public interface IArticlesRepository
    {
        Article FetchArticle(Guid articleID);
    }
}