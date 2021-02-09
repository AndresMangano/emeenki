namespace Hermes.Core
{
    public class UserProfilePhotoChangedEvent
    {
        public string ProfilePhotoURL { get; }

        public UserProfilePhotoChangedEvent(string profilePhotoURL) {
            ProfilePhotoURL = profilePhotoURL;
        }
    }
}