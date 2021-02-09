namespace Hermes.Core
{
    public class UserDetailsDTO
    {
        public string UserID { get; set; }
        public string ProfilePhotoURL { get; set; }
        public string NativeLanguageID { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public string SignInType { get; set; }
    }
}