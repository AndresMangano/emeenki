namespace Hermes.Core
{
    public class RoomUnrestrictedEvent
    {
        public string UserID { get; }

        public RoomUnrestrictedEvent(string userID)
        {
            UserID = userID;
        }
    }
}