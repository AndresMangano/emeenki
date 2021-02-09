namespace Hermes.Worker.Core.Model.Events.Article
{
    public class ArticleArchivedEvent
    {
        public string UserID { get; }
        
        public ArticleArchivedEvent(string userID)
        {
            UserID = userID;
        }
    }
}