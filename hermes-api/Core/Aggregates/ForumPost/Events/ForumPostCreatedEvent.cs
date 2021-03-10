namespace Hermes.Core
{
    public record ForumPostCreatedEvent(
        string Title,
        string Text,
        string LanguageID,
        string UserID
    );
}