namespace Hermes.Worker.Core.Model.Events.User
{
    public class UserRegisteredEvent
    {
        public string Password { get; }
        public string ProfilePhotoURL { get; }
        public string LanguageID { get; }
        public string Rights { get; }
        public string Country { get; }

        public UserRegisteredEvent(string password, string profilePhotoURL, string languageID, string rights, string country)
        {
            Password = password;
            ProfilePhotoURL = profilePhotoURL;
            LanguageID = languageID;
            Rights = rights;
            Country = country;
        }
    }
}