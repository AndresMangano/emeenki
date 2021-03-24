using System;

namespace Hermes.Core
{
    public record ForumPostCommentDeletedEvent(
        Guid ForumPostCommentID
    );
}