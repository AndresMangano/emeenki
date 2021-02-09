namespace Hermes.Worker.Core.Model.Events.User
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