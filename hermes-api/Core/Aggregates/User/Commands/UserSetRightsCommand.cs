using System;

namespace Hermes.Core
{
    public class UserSetRightsCommand
    {
        public string UserID { get; set; }
        public string NewRights { get; set; }
    }
}