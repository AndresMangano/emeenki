using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.User
{
    public record UserRegisteredEvent(
        EventHeader Header,
        string ID,
        string Password,
        string ProfilePhotoURL,
        string LanguageID,
        string Rights,
        string Country
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertUser(
                userID: ID,
                rights: Rights,
                profilePhotoURL: ProfilePhotoURL,
                nativeLanguageID: LanguageID,
                country: Country,
                signInType: "password"
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.USER_UPDATED, ID, "users");
        }
    }
}