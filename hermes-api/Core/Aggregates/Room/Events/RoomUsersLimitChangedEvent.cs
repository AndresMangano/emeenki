namespace Hermes.Core
{
    public class RoomUsersLimitChangedEvent
    {
        public short NewUsersLimit { get; }

        public RoomUsersLimitChangedEvent(short newUsersLimit)
        {
            NewUsersLimit = newUsersLimit;
        }
    }
}