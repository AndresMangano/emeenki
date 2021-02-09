namespace Hermes.Core
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