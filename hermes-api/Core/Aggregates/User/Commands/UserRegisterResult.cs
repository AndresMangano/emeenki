namespace Hermes.Core
{
    public class UserRegisterResult
    {
        public string UserID { get; }

        public UserRegisterResult(string userID) {
            UserID = userID;
        }
    }
}