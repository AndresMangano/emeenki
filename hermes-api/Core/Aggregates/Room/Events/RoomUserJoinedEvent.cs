namespace Hermes.Core
{
    public record RoomUserJoinedEvent(
        string UserID,
        string Permission
    );
}