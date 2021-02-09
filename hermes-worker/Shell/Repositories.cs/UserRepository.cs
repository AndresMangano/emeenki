using System;
using Dapper;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IUserRepository
    {
        public void DeleteUser(string userID)
        {
            _connection.Execute(@"
                DELETE FROM Query_User
                WHERE UserID = @userID",
                new {
                    userID
                },
                transaction: _transaction
            );
        }

        public void InsertUser(string userID, string rights, string profilePhotoURL, string nativeLanguageID, string country, string signInType)
        {
            _connection.Execute(@"
                INSERT INTO Query_User(UserID, Rights, ProfilePhotoURL, NativeLanguageID, Country, SignInType)
                VALUES(@userID, @rights, @profilePhotoURL, @nativeLanguageID, @country, @signInType)
                    ON DUPLICATE KEY UPDATE UserID = @userID",
                new {
                    userID,
                    rights,
                    profilePhotoURL,
                    nativeLanguageID,
                    country,
                    signInType
                },
                transaction: _transaction
            );
        }

        public void UpdateUser(string userID, DbUpdate<string> rights = null, DbUpdate<string> profilePhotoURL = null, DbUpdate<string> nativeLanguageID = null,
        DbUpdate<string> description = null, DbUpdate<string> country = null)
        {
            if (rights != null || profilePhotoURL != null || nativeLanguageID != null || description != null || country != null) {
                _connection.Execute(@"
                    UPDATE Query_User
                    SET Rights = CASE @setRights WHEN 1 THEN @rights ELSE Rights END,
                        ProfilePhotoURL = CASE @setProfilePhotoURL WHEN 1 THEN @profilePhotoURL ELSE ProfilePhotoURL END,
                        NativeLanguageID = CASE @setNativeLanguageID WHEN 1 THEN @nativeLanguageID ELSE NativeLanguageID END,
                        Description = CASE @setDescription WHEN 1 THEN @description ELSE Description END,
                        Country = CASE @setCountry WHEN 1 THEN @country ELSE Country END
                    WHERE UserID = @userID",
                    new {
                        userID,
                        setRights = rights != null,
                        rights = rights?.Value,
                        setProfilePhotoURL = profilePhotoURL != null,
                        profilePhotoURL = profilePhotoURL?.Value,
                        setNativeLanguageID = nativeLanguageID != null,
                        nativeLanguageID = nativeLanguageID?.Value,
                        setDescription = description != null,
                        description = description?.Value,
                        setCountry = country != null,
                        country = country?.Value
                    },
                    transaction: _transaction
                );
            }
        }
    }
}