using System;

namespace Hermes.Core
{
    public record ForumPostDeleteCommentCommand(
        Guid ForumPostID,
        Guid ForumPostCommentID
    );
}