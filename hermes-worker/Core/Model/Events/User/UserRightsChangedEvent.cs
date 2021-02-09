namespace Hermes.Worker.Core.Model.Events.User
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