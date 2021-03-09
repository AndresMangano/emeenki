using System;

namespace Hermes.Core
{
    public record UserPostAddedEvent(
        Guid UserPostID,
        string Text,
        string UserID,
        Guid? ChildUserPostID
    );
}