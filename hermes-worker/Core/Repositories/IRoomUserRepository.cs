namespace Hermes.Worker.Core.Repositories
{
    public interface IRoomUserRepository
    {
        void InsertRoomUser(string roomID, string userID, string permission);
        void DeleteRoomUser(string roomID, string userID);
    }
}