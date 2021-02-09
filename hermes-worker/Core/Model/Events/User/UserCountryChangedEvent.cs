namespace Hermes.Worker.Core.Model.Events.User
{
    public class UserCountryChangedEvent
    {
        public string Country { get; }
        
        public UserCountryChangedEvent(string country)
        {
            Country = country;
        }
    }
}