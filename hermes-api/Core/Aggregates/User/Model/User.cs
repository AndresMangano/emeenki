using System.Collections.Generic;

namespace Hermes.Core
{
    public class User : IAggregateRoot<string>
    {
        public string ID { get; set; }
        public int Version { get; set; }
        public bool Created { get; set; }
        public bool Deleted { get; set; }
        public string ProfilePhotoURL { get; set; }
        public string LanguageID { get; set; }
        public ISignInMethod SignInMethod { get; set; }
        public UserRights Rights { get; set; }
        public bool Banned { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public List<UserPost> Posts { get; set; }
    }
}