using System;

namespace Hermes.Core
{
    public record ForumPostEditCommand(
        Guid ForumPostID,
        string Title,
        string Text,
        string LanguageID
    );
}