namespace Hermes.Core
{
    public record ForumPostEditedEvent(
        string Title,
        string Text,
        string LanguageID
    );
}