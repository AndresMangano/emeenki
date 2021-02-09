namespace Hermes.Core
{
    public class UserDescriptionChangedEvent
    {
        public string Description { get; }
        
        public UserDescriptionChangedEvent(string description)
        {
            Description = description;
        }
    }
}