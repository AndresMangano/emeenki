namespace Hermes.Core
{
    public class RoomUsersDTO
    {
        public string RoomID { get; set; }
        public string Username { get; set; }
        public string NativeLanguageID { get; set; }
        public string PhotoURL { get; set; }
        public string Rights { get; set; }
        public bool Pending { get; set; }
    }
}