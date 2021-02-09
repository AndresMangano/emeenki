namespace Hermes.Worker.Core.Model.Events.Room
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