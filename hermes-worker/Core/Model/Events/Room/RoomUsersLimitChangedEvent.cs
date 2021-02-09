namespace Hermes.Worker.Core.Model.Events.Room
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