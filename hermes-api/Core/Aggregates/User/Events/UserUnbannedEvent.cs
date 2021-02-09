namespace Hermes.Core
{
    public class UserUnbannedEvent
    {
        public string AdminUserID { get; }

        public UserUnbannedEvent(string adminUserID)
        {
            AdminUserID = AdminUserID;
        }
    }
}