using System.Linq;
using Hermes.Core;
using Xunit;

namespace Hermes.Core.Tests.Users
{
    public class UserRegisteredWithPasswordCommandTests
    {
        readonly TestDomainInterpreter _env;
        public UserRegisteredWithPasswordCommandTests()
        {
            _env = new TestDomainInterpreter();
        }

        [Fact]
        public void GivenValidRequest_WhenUserRegisteredWithPassword_ThenStoreUserInDatabase()
        {
            // ARRANGE
            var command = new UserRegisterWithPasswordCommand {
                UserID = "hoxon",
                Password = "somepass",
                ProfilePhotoURL = "https://photo.png",
                LanguageID = "SPA"
            };
            // ACT
            UserCommands.Execute(_env, command);
            // ASSERT
            var user = _env.Users.SingleOrDefault(u => u.ID == "hoxon");
            Assert.NotNull(user);
            Assert.Equal("hoxon", user.ID);
            Assert.True(user.SignInMethod is PasswordSignIn psi && psi.Password == "somepass");
            Assert.Equal("https://photo.png", user.ProfilePhotoURL);
            Assert.Equal("SPA", user.LanguageID);
        }
    }
}
