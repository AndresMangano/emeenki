namespace Hermes.Core
{
    public class RoomOpenResult
    {
        public string RoomID { get; }
        public RoomOpenResult(string roomID)
        {
            RoomID = roomID;
        }
    }
}