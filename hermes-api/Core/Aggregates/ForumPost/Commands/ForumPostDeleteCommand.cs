using System;

namespace Hermes.Core
{
    public record ForumPostDeleteCommand(
        Guid ForumPostID
    );
}