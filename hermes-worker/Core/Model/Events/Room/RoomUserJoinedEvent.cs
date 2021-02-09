namespace Hermes.Worker.Core.Model.Events.Room
{
    public class RoomUserJoinedEvent
    {
        public string UserID { get; }
        public string Permission { get; }

        public RoomUserJoinedEvent(string userID, string permission)
        {
            UserID = userID;
            Permission = permission;
        }
    }
}