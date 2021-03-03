using Hermes.Core;
using Hermes.Core.Ports;
using Newtonsoft.Json;

namespace Hermes.Shell
{
    public partial class DomainInterpreter : IUsersRepository
    {
        void InitUsersRepository()
        {
            UsersRepository = new EventRepository<string, User>(new SQLEventStorage<string>(
                _connection.ConnectionString,
                "User",
                ParseUserEvent
            ));
        }
        object ParseUserEvent(string eventName, string payload)
        {
            switch(eventName) {
                case "unbanned": return JsonConvert.DeserializeObject<UserUnbannedEvent>(payload);
                case "deleted": return JsonConvert.DeserializeObject<UserDeletedEvent>(payload);
                case "banned": return JsonConvert.DeserializeObject<UserBannedEvent>(payload);
                case "rights.changed": return JsonConvert.DeserializeObject<UserRightsChangedEvent>(payload);
                case "loggedOut": return JsonConvert.DeserializeObject<UserLoggedOutEvent>(payload);
                case "loggedIn": return JsonConvert.DeserializeObject<UserLoggedInEvent>(payload);
                case "registered": return JsonConvert.DeserializeObject<UserRegisteredEvent>(payload);
                case "registered.withGoogle": return JsonConvert.DeserializeObject<UserRegisteredWithGoogleEvent>(payload);
                case "profilePhotoChanged": return JsonConvert.DeserializeObject<UserProfilePhotoChangedEvent>(payload);
                case "post.added": return JsonConvert.DeserializeObject<UserPostAddedEvent>(payload);
                case "post.deleted": return JsonConvert.DeserializeObject<UserPostDeletedEvent>(payload);
                case "language.changed": return JsonConvert.DeserializeObject<UserLanguageChangedEvent>(payload);
                case "description.changed": return JsonConvert.DeserializeObject<UserDescriptionChangedEvent>(payload);
                case "country.changed": return JsonConvert.DeserializeObject<UserCountryChangedEvent>(payload);
                case "password.changed": return JsonConvert.DeserializeObject<UserPasswordChangedEvent>(payload);
                default:
                    throw new InfrastructureException("Unknown user event");
            }
        }
        long? ApplyUserEvent(UserEvent @event) {
            var index = UsersRepository.StoreEvent(@event);
            if (index != null) {
                SendMessage(
                    "user_events",
                    index.Value,
                    @event.Metadata,
                    @event.Payload,
                    obj => obj.Add("ID", @event.Metadata.ID));
            }
            return index;
        }
        
        public User FetchUser(string id) => UsersRepository.Fetch(id.ToLower(), UserEvents.Apply);
    }
}