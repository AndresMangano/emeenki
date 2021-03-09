using System;

namespace Hermes.Core
{
    public record UserDeletedEvent(
        Guid SessionID
    );
}