using System;

namespace Hermes.Core
{
    public record UserLoggedOutEvent(
        Guid SessionID
    );
}