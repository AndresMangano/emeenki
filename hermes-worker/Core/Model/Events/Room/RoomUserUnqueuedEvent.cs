namespace Hermes.Worker.Core.Model.Events.Room
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