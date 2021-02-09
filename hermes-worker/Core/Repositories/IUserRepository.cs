using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Core.Repositories
{
    public interface IUserRepository
    {
        void InsertUser(string userID, string rights, string profilePhotoURL, string nativeLanguageID, string country, string signInType);
        void DeleteUser(string userID);
        void UpdateUser(string userID,
            DbUpdate<string> rights = null,
            DbUpdate<string> profilePhotoURL = null,
            DbUpdate<string> nativeLanguageID = null,
            DbUpdate<string> description = null,
            DbUpdate<string> country = null);
    }
}