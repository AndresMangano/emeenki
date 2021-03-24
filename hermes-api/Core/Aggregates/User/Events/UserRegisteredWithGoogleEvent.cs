namespace Hermes.Core
{
    public record UserRegisteredWithGoogleEvent(
        string GoogleEmail,
        string ProfilePhotoURL,
        string LanguageID,
        string Rights,
        string Country
    );
}