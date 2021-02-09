namespace Hermes.Core
{
    public class RoomClosedEvent
    {
        public string UserID { get; }

        public RoomClosedEvent(string userID)
        {
            UserID = userID;
        }
    }
}