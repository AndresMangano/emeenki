using System;

namespace Hermes.Core
{
    public record UserLoggedInEvent(
        Guid SessionID
    );
}