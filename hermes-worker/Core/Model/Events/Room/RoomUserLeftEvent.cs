namespace Hermes.Worker.Core.Model.Events.Room
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