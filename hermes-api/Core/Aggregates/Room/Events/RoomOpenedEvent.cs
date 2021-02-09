using System;

namespace Hermes.Core
{
    public class RoomOpenedEvent
    {
        public Guid Token { get; }
        public string LanguageID1 { get; }
        public string LanguageID2 { get; }
        public short UsersLimit { get; }
        public bool Restricted { get; }
        public string UserID { get; }

        public RoomOpenedEvent(Guid token, string languageID1, string languageID2, short usersLimit, bool restricted, string userID)
        {
            Token = token;
            LanguageID1 = languageID1;
            LanguageID2 = languageID2;
            UsersLimit = usersLimit;
            Restricted = restricted;
            UserID = userID;
        }
    }
}