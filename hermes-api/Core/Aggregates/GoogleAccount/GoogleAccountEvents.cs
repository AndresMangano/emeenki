namespace Hermes.Core
{
    public class GoogleAccountEvents
    {
        public static void Apply(GoogleAccount googleAccount, DomainEvent<string, object> @event)
        {
            switch (@event.Payload) {
                case GoogleAccountTakenEvent e:
                    googleAccount.ID = @event.Metadata.ID;
                    googleAccount.Created = true;
                    googleAccount.Deleted = false;
                    googleAccount.UserID = e.UserID;
                    break;
                case GoogleAccountReleasedEvent e:
                    googleAccount.Deleted = true;
                    break;
            }
            googleAccount.Version = @event.Metadata.Version;
        }
    }
}