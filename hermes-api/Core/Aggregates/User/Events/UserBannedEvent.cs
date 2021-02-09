namespace Hermes.Core
{
    public class UserBannedEvent
    {
        public string Reason { get; }
        public string AdminUserID { get; }

        public UserBannedEvent(string reason, string adminUserID)
        {
            Reason = reason;
            AdminUserID = AdminUserID;
        }
    }
}