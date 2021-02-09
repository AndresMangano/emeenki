namespace Hermes.Core
{
    public class UserRegisterWithGoogleCommand
    {
        public string UserID { get; set; }
        public string GoogleEmail { get; set; }
        public string ProfilePhotoURL { get; set; }
        public string LanguageID { get; set; }
        public string Country { get; set; }
    }
}