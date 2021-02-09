namespace Hermes.Worker.Core.Model.Events.User
{
    public class UserProfilePhotoChangedEvent
    {
        public string ProfilePhotoURL { get; }

        public UserProfilePhotoChangedEvent(string profilePhotoURL) {
            ProfilePhotoURL = profilePhotoURL;
        }
    }
}