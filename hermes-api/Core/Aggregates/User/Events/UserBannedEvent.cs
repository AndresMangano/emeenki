namespace Hermes.Core
{
    public record UserBannedEvent(
        string Reason,
        string AdminUserID
    );
}