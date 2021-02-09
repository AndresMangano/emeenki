namespace Hermes.Core
{
    public class RoomUserLeftEvent
    {
        public string UserID { get; }

        public RoomUserLeftEvent(string userID)
        {
            UserID = userID;
        }
    }
}