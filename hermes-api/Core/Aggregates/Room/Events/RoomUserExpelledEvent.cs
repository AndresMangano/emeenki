namespace Hermes.Core
{
    public class RoomUserExpelledEvent
    {
        public string UserID { get; }

        public RoomUserExpelledEvent(string userID)
        {
            UserID = userID;
        }
    }
}