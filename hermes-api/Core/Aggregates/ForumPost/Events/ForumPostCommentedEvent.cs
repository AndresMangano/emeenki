using System;

namespace Hermes.Core
{
    public record ForumPostCommentedEvent(
        Guid ForumPostCommentID,
        string Text,
        string UserID
    );
}