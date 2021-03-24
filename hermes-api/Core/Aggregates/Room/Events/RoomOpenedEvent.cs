using System;

namespace Hermes.Core
{
    public record RoomOpenedEvent(
        Guid Token,
        string LanguageID1,
        string LanguageID2,
        short UsersLimit,
        bool Restricted,
        string UserID
    );
}