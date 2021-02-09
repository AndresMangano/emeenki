using Hermes.Core;
using Newtonsoft.Json;

namespace Hermes.Shell
{
    public partial class DomainInterpreter : IGoogleAccountRepository
    {
        void InitGoogleAccountRepository()
        {
            _googleAccountsRepository = new EventRepository<string, GoogleAccount>(new SQLEventStorage<string>(
                _connection.ConnectionString,
                "GoogleAccount",
                ParseGoogleAccountEvent
            ));
        }
        object ParseGoogleAccountEvent(string eventName, string payload)
        {
            switch(eventName) {
                case "taken": return JsonConvert.DeserializeObject<GoogleAccountTakenEvent>(payload);
                case "released": return JsonConvert.DeserializeObject<GoogleAccountReleasedEvent>(payload);
                default:
                    throw new InfrastructureException("Unknown Article Event");
            }
        }
        long? ApplyGoogleAccountEvent(GoogleAccountEvent @event) {
            var index = _googleAccountsRepository.StoreEvent(@event);
            return index;
        }

        public GoogleAccount FetchGoogleAccount(string googleAccountID) => _googleAccountsRepository.Fetch(googleAccountID, GoogleAccountEvents.Apply);
    }
}