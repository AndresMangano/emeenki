namespace Hermes.Worker.Core.Repositories
{
    public interface IRoomQueueRepository
    {
        void InsertRoomQueue(string roomID, string userID);
        void DeleteRoomQueue(string roomID, string userID);
    }
}