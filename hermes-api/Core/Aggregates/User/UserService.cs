using System;
using System.Net.Mail;

namespace Hermes.Core
{
    public static class UserService
    {
        public static void ValidateExistence(User user) {
            if (!user.Created || user.Deleted)
                throw new DomainException("The User does not exist");
        }
        public static void ValidateInexistent(User user) {
            if (user.Created && user.Deleted)
                throw new DomainException("The User was deleted");
            else if (user.Created)
                throw new DomainException("The User already exists");
        }

        public static void ValidateUserID(string userID) {
            if (string.IsNullOrEmpty(userID))
                throw new DomainException("Invalid username");
            else if (!Char.IsLetter(userID, 0))
                throw new DomainException("Invalid username");
        }

        public static void ValidatePasswordFormat(string password) {
            if (password.Length < 6)
                throw new DomainException("Invalid password");
        }

        public static void ValidateRootUser(string userID) {
            if (userID != "root")
                throw new DomainException("The user is not the root");
        }

        public static void ValidateAdminRights(User user) {
            if (user.Rights != UserRights.ADMIN)
                throw new DomainException("Lack of admin rights");
        }
        
        public static void ValidateRightsChange(User user, UserRights newRights) {
            if (user.Rights == newRights)
                throw new DomainException("There is no change in the user rights");
        }

        public static void ValidatePassword(User user, string password) {
            if (user.SignInMethod is PasswordSignIn == false) {
                throw new DomainException("Invalid Sign In method");
            } else if (user.SignInMethod is PasswordSignIn psi && psi.Password != password) {
                throw new DomainException("Wrong password");
            }
        }
        public static void ValidateGoogleEmail(User user, string email) {
            if (user.SignInMethod is GoogleSignIn == false) {
                throw new DomainException("Invalid Sign in method");
            } else if(user.SignInMethod is GoogleSignIn gsi && gsi.Email != email) {
                throw new DomainException("Invalid Google account");
            }
        }

        public static UserRights ParseUserRights(string rights) {
            switch (rights) {
                case "admin": return UserRights.ADMIN;
                case "user": return UserRights.USER;
                default:
                    return UserRights.USER;
            }
        }

        public static string GetUserRightValue(UserRights rights) {
            switch (rights) {
                case UserRights.ADMIN: return "admin";
                case UserRights.USER: return "user";
                default:
                    return "user";
            }
        }
    }
}