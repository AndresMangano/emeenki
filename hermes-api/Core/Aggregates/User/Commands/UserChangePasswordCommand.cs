namespace Hermes.Core
{
    public class UserChangePasswordCommand
    {
        public string ActualPassword { get; }
        public string NewPassword { get; }

        public UserChangePasswordCommand(string actualPassword, string newPassword)
        {
            ActualPassword = actualPassword;
            NewPassword = newPassword;
        }
    }
}