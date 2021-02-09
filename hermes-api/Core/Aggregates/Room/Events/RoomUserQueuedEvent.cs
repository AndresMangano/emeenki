namespace Hermes.Core
{
    public class RoomUserQueuedEvent
    {
        public string UserID { get; }

        public RoomUserQueuedEvent(string userID)
        {
            UserID = userID;
        }
    }
}