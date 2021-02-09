namespace Hermes.Core
{
    public class ArticleTemplateDeletedEvent
    {
        public string UserID { get; }

        public ArticleTemplateDeletedEvent(string userID)
        {
            UserID = userID;
        }
    }
}