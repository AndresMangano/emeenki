namespace Hermes.Worker.Core.Model.Events.Room
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