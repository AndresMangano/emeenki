using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Room
{
    public record RoomOpenedEvent(
        EventHeader<string> Header,
        Guid Token,
        string LanguageID1,
        string LanguageID2,
        short UsersLimit,
        bool Restricted,
        string UserID
    ) : IEvent<string>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertRoom(
                roomID: Header.ID,
                languageID1: LanguageID1,
                languageID2: LanguageID2,
                closed: false,
                restricted: Restricted,
                usersLimit: UsersLimit
            );
            interpreter.InsertRoomUser(
                roomID: Header.ID,
                userID: UserID,
                permission: "admin"
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, Header.ID, "rooms");
        }
    }
}