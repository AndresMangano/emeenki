namespace Hermes.Core
{
    public class RoomRestrictedEvent
    {
        public string UserID { get; }

        public RoomRestrictedEvent(string userID)
        {
            UserID = userID;
        }
    }
}