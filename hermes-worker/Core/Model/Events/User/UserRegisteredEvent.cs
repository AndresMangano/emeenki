using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.User
{
    public record UserRegisteredEvent(
        EventHeader<string> Header,
        string Password,
        string ProfilePhotoURL,
        string LanguageID,
        string Rights,
        string Country
    ) : IEvent<string>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertUser(
                userID: Header.ID,
                rights: Rights,
                profilePhotoURL: ProfilePhotoURL,
                nativeLanguageID: LanguageID,
                country: Country,
                signInType: "password"
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.USER_UPDATED, Header.ID, "users");
        }
    }
}