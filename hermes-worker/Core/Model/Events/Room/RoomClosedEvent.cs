namespace Hermes.Worker.Core.Model.Events.Room
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