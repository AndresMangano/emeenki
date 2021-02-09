namespace Hermes.Core
{
    public class UserRegisterWithPasswordCommand
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string ProfilePhotoURL { get; set; }
        public string LanguageID { get; set; }
        public string Country { get; set; }
    }
}