namespace Hermes.Core
{
    public class UserRegisteredWithGoogleEvent
    {
        public string GoogleEmail { get; }
        public string ProfilePhotoURL { get; }
        public string LanguageID { get; }
        public string Rights { get; }
        public string Country { get; }

        public UserRegisteredWithGoogleEvent(string googleEmail, string profilePhotoURL, string languageID, string rights, string country)
        {
            GoogleEmail = googleEmail;
            ProfilePhotoURL = profilePhotoURL;
            LanguageID = languageID;
            Rights = rights;
            Country = country;
        }
    }
}