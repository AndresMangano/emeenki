namespace Hermes.Core
{
    public record UserRegisteredEvent(
        string Password,
        string ProfilePhotoURL,
        string LanguageID,
        string Rights,
        string Country
    );
}