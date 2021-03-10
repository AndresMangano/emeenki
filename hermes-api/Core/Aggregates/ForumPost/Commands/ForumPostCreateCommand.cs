namespace Hermes.Core
{
    public record ForumPostCreateCommand(
        string Title,
        string Text,
        string LanguageID
    );
}