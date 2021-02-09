namespace Hermes.Core
{
    public static class ArticleTemplateService
    {
        public static void ValidateExistence(ArticleTemplate articleTemplate) {
            if (!articleTemplate.Created || articleTemplate.Deleted)
                throw new DomainException("The Article does not exist");
        }
    }
}