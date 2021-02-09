namespace Hermes.Core.Ports
{
    public interface IRoomsRepository
    {
        Room FetchRoom(string roomID);
    }
}