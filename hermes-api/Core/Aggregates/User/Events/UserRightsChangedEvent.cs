namespace Hermes.Core
{
    public class UserRightsChangedEvent
    {
        public string NewRights { get; }

        public UserRightsChangedEvent(string newRights)
        {
            NewRights = newRights;
        }
    }
}