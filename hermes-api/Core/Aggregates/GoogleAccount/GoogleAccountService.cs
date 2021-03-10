using System;
using Hermes.Core.Ports;

namespace Hermes.Core
{
    public static class GoogleAccountService
    {
        public static void TakeGoogleAccount<IO>(IO io, string googleAccountID, string userID)
        where IO : IGoogleAccountRepository, IEventsRepository
        {
            var googleAccount = io.FetchGoogleAccount(googleAccountID);
            var isAvailable = !googleAccount.Created || googleAccount.Deleted;
            if (!isAvailable) {
                throw new DomainException("Google Account associated to another user");
            } else if (googleAccount.UserID != userID) {
                io.StoreEvent(new GoogleAccountEvent(
                    id: googleAccountID,
                    version: googleAccount.Version + 1,
                    eventName: "taken",
                    payload: new GoogleAccountTakenEvent(
                        userID
                    )
                ));
            }
        }
        public static void ReleaseGoogleAccount<IO>(IO io, string googleAccountID)
        where IO : IGoogleAccountRepository, IEventsRepository
        {
            var googleAccount = io.FetchGoogleAccount(googleAccountID);
            if (googleAccount.Created && !googleAccount.Deleted) {
                io.StoreEvent(new GoogleAccountEvent(
                    id: googleAccountID,
                    version: googleAccount.Version + 1,
                    eventName: "released",
                    payload: new GoogleAccountReleasedEvent()
                ));
            }
        }
    }
}