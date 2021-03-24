using System;

namespace Hermes.Core
{
    public record ForumPostCommentCommand(
        Guid ForumPostID,
        string Text
    );
}