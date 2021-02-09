namespace Hermes.Core
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