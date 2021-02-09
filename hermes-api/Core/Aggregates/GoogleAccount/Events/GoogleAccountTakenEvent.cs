namespace Hermes.Core
{
    public class GoogleAccountTakenEvent
    {
        public string UserID { get; }
        public GoogleAccountTakenEvent(string userID)
        {
            UserID = userID;
        }
    }
}