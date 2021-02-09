namespace Hermes.Core
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