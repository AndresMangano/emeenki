using System;

namespace Hermes.Core
{
    public record RoomTokenRenewedEvent(
        Guid Token
    );
}