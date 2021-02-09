namespace Hermes.Worker.Core.Model.Events.ArticleTemplate
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