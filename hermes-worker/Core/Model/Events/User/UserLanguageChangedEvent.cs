namespace Hermes.Worker.Core.Model.Events.User
{
    public class UserLanguageChangedEvent
    {
        public string NativeLanguageID { get; }
        
        public UserLanguageChangedEvent(string nativeLanguageID)
        {
            NativeLanguageID = nativeLanguageID;
        }
    }
}