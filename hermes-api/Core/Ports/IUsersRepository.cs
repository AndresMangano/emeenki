namespace Hermes.Core.Ports
{
    public interface IUsersRepository
    {
        User FetchUser(string userID);
    }
}