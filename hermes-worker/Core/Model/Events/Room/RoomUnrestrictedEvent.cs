namespace Hermes.Worker.Core.Model.Events.Room
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