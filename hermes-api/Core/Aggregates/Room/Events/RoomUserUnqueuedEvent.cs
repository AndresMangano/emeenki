namespace Hermes.Core
{
    public class RoomUserUnqueuedEvent
    {
        public string UserID { get; }

        public RoomUserUnqueuedEvent(string userID)
        {
            UserID = userID;
        }
    }
}