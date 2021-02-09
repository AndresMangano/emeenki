namespace Hermes.Worker.Core.Model.Events.Room
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