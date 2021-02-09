namespace Hermes.Core
{
    public class UserPasswordChangedEvent
    {
        public string Password { get; }
        
        public UserPasswordChangedEvent(string password)
        {
            Password = password;
        }
    }
}