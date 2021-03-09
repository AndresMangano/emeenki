using System;

namespace Hermes.Core
{
    public record UserPostDeletedEvent(
        Guid UserPostID,
        Guid? ChildUserPostID,
        string SenderUserID
    );
}