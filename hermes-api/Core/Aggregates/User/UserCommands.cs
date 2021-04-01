using System;
using System.Linq;
using Hermes.Core.Ports;

namespace Hermes.Core
{
    public static class UserCommands
    {
        public static void Execute<IO>(IO io, UserUnbanCommand cmd, string userID)
        where IO : IEventsRepository, IUsersRepository
        {
            var user = io.FetchUser(cmd.UserID);
            var adminUser = io.FetchUser(userID);
            UserService.ValidateAdminRights(adminUser);
            if (!user.Banned)
                throw new DomainException("User is not banned");
            io.StoreEvent(new UserEvent (
                id: user.ID,
                version: user.Version + 1,
                eventName: "unbanned",
                payload: new UserUnbannedEvent(
                    adminUser.ID)));
        }

        public static UserRegisterResult Execute<IO>(IO io, UserRegisterWithPasswordCommand cmd)
        where IO : IEventsRepository, IUsersRepository, ILanguagesRepository
        {
            var user = io.FetchUser(cmd.UserID);
            var language = io.FetchLanguage(cmd.LanguageID);
            LanguageService.ValidateExistence(language);
            UserService.ValidateInexistent(user);
            UserService.ValidateUserID(cmd.UserID);
            UserService.ValidatePasswordFormat(cmd.Password);
            io.StoreEvent(new UserEvent (
                id: cmd.UserID,
                version: user.Version + 1,
                eventName: "registered",
                payload: new UserRegisteredEvent(
                    cmd.Password,
                    cmd.ProfilePhotoURL,
                    language.ID,
                    "user",
                    cmd.Country)));
            return new UserRegisterResult(cmd.UserID);
        }

        public static UserRegisterResult Execute<IO>(IO io, UserRegisterWithGoogleCommand cmd)
        where IO : IEventsRepository, IUsersRepository, ILanguagesRepository, IGoogleAccountRepository
        {
            var user = io.FetchUser(cmd.UserID);
            var language = io.FetchLanguage(cmd.LanguageID);
            LanguageService.ValidateExistence(language);
            UserService.ValidateInexistent(user);
            UserService.ValidateUserID(cmd.UserID);
            GoogleAccountService.TakeGoogleAccount(io, cmd.GoogleEmail, cmd.UserID);
            io.StoreEvent(new UserEvent (
                id: cmd.UserID,
                version: user.Version + 1,
                eventName: "registered.withGoogle",
                payload: new UserRegisteredWithGoogleEvent(
                    cmd.GoogleEmail,
                    cmd.ProfilePhotoURL,
                    language.ID,
                    "user",
                    cmd.Country
                )
            ));
            return new UserRegisterResult(cmd.UserID);
        }

        public static void Execute<IO>(IO io, UserDeleteCommand cmd, string userID)
        where IO : IEventsRepository, IUsersRepository
        {
            var user = io.FetchUser(userID);
            if (user.Deleted)
                throw new DomainException("The user was already deleted");
            
            io.StoreEvent(new UserEvent (
                id: user.ID,
                version: user.Version + 1,
                eventName: "deleted",
                payload: new UserDeletedEvent(
                    Guid.NewGuid())));
        }

        public static void Execute<IO>(IO io, UserBanCommand cmd, string userID)
        where IO : IEventsRepository, IUsersRepository
        {
            var user = io.FetchUser(cmd.UserID);
            UserService.ValidateExistence(user);
            var adminUser = io.FetchUser(userID);
            UserService.ValidateAdminRights(adminUser);
            if (user.Banned)
                throw new DomainException("User already banned");
            io.StoreEvent(new UserEvent (
                id: user.ID,
                version: user.Version + 1,
                eventName: "banned",
                payload: new UserBannedEvent(
                    cmd.Reason,
                    adminUser.ID)));
        }

        public static void Execute<IO>(IO io, UserSetRightsCommand cmd, string userID)
        where IO : IEventsRepository, IUsersRepository
        {
            var user = io.FetchUser(cmd.UserID);
            UserService.ValidateRootUser(userID);
            UserService.ValidateRightsChange(user, UserService.ParseUserRights(cmd.NewRights));
            io.StoreEvent(new UserEvent (
                id: user.ID,
                version: user.Version + 1,
                eventName: "rights.changed",
                payload: new UserRightsChangedEvent(
                    cmd.NewRights)));
        }

        public static UserLogInResult Execute<IO>(IO io, UserLogInWithPasswordCommand cmd)
        where IO : IEventsRepository, IUsersRepository
        {
            var user = io.FetchUser(cmd.UserID);
            UserService.ValidateExistence(user);
            UserService.ValidatePassword(user, cmd.Password);
            return new UserLogInResult {
                UserID = user.ID,
                ProfilePhotoURL = user.ProfilePhotoURL,
                Rights = UserService.GetUserRightValue(user.Rights)
            };
        }

        public static UserLogInResult Execute<IO>(IO io, UserLogInWithGoogleCommand cmd)
        where IO : IEventsRepository, IUsersRepository, IGoogleAccountRepository
        {
            var googleAccount = io.FetchGoogleAccount(cmd.GoogleEmail);
            if (googleAccount == null || !googleAccount.Created || googleAccount.Deleted) {
                throw new DomainException("Invalid Google Account");
            } else {
                var user = io.FetchUser(googleAccount.UserID);
                UserService.ValidateExistence(user);
                UserService.ValidateGoogleEmail(user, cmd.GoogleEmail);
                return new UserLogInResult {
                    UserID = user.ID,
                    ProfilePhotoURL = user.ProfilePhotoURL,
                    Rights = UserService.GetUserRightValue(user.Rights)
                };
            }
        }

        public static void Execute<IO>(IO io, UserChangeProfilePhotoCommand cmd, string userID)
        where IO : IEventsRepository, IUsersRepository
        {
            var user = io.FetchUser(userID);
            io.StoreEvent(new UserEvent (
                id: user.ID,
                version: user.Version + 1,
                eventName: "profilePhotoChanged",
                payload: new UserProfilePhotoChangedEvent(
                    cmd.ProfilePhotoURL)));
        }
        public static void Execute<IO>(IO io, UserAddPostCommand cmd, string userID)
        where IO : IEventsRepository, IUsersRepository
        {
            var user = io.FetchUser(cmd.UserID);
            UserService.ValidateExistence(user);
            if (cmd.ParentUserPostID != null && user.Posts.SingleOrDefault(up => up.UserPostID == cmd.ParentUserPostID) == null) {
                throw new DomainException("Invalid parent post");
            }
            io.StoreEvent(new UserEvent(
                id: user.ID,
                version: user.Version + 1,
                eventName: "post.added",
                payload: new UserPostAddedEvent(
                    cmd.ParentUserPostID == null ? Guid.NewGuid() : cmd.ParentUserPostID.Value,
                    cmd.Text,
                    userID,
                    cmd.ParentUserPostID == null ? (Guid?)null : Guid.NewGuid()
                )
            ));
        }
        public static void Execute<IO>(IO io, UserDeletePostCommand cmd, string userID)
        where IO : IEventsRepository, IUsersRepository
        {
            var user = io.FetchUser(cmd.UserID);
            UserService.ValidateExistence(user);
            var post = user.Posts.SingleOrDefault(p => p.UserPostID == cmd.UserPostID);
            var childPost = post.Replies.SingleOrDefault(r => r.UserPostID == cmd.ChildUserPostID);
            var ṕostUserID = childPost == null ? post.UserID : childPost.UserID;
            if (post == null)
                throw new DomainException("User post not found");
            else if (ṕostUserID != userID && userID != cmd.UserID)
                throw new DomainException("You cannot delete this post");
            else if (cmd.ChildUserPostID != null && post.Replies.SingleOrDefault(p => p.UserPostID == cmd.ChildUserPostID) == null) {
                throw new DomainException("User post not found");
            }
            else
                io.StoreEvent(new UserEvent(
                    id: cmd.UserID,
                    version: user.Version + 1,
                    eventName: "post.deleted",
                    payload: new UserPostDeletedEvent(
                        cmd.UserPostID,
                        cmd.ChildUserPostID,
                        userID
                )));
        }
        public static void Execute<IO>(IO io, UserChangeLanguageCommand cmd, string userID)
        where IO : IEventsRepository, IUsersRepository
        {
            var user = io.FetchUser(userID);
            UserService.ValidateExistence(user);
            if (user.LanguageID != cmd.NativeLanguageID)
                io.StoreEvent(new UserEvent(
                    id: userID,
                    version: user.Version + 1,
                    eventName: "language.changed",
                    payload: new UserLanguageChangedEvent(
                        cmd.NativeLanguageID
                    )
                ));
        }
        public static void Execute<IO>(IO iO, UserChangeDescriptionCommand command, string userID)
        where IO : IEventsRepository, IUsersRepository
        {
            var user = iO.FetchUser(userID);
            UserService.ValidateExistence(user);
            if (user.Description != command.Description)
                iO.StoreEvent(new UserEvent(
                    id: userID,
                    version: user.Version + 1,
                    eventName: "description.changed",
                    payload: new UserDescriptionChangedEvent(
                        command.Description
                    )
                ));
        }
        public static void Execute<IO>(IO iO, UserChangeCountryCommand command, string userID)
        where IO : IEventsRepository, IUsersRepository
        {
            var user = iO.FetchUser(userID);
            UserService.ValidateExistence(user);
            if (user.Country != command.Country)
                iO.StoreEvent(new UserEvent(
                    id: userID,
                    version: user.Version + 1,
                    eventName: "country.changed",
                    payload: new UserCountryChangedEvent(
                        command.Country
                    )
                ));
        }

        public static void Execute<IO>(IO iO, UserChangePasswordCommand command, string userID)
        where IO : IEventsRepository, IUsersRepository
        {
            var user = iO.FetchUser(userID);
            UserService.ValidateExistence(user);
            if (user.SignInMethod is PasswordSignIn passwordSignIn) {
                if (passwordSignIn.Password == command.ActualPassword) {
                    iO.StoreEvent(new UserEvent(
                        id: userID,
                        version: user.Version + 1,
                        eventName: "password.changed",
                        payload: new UserPasswordChangedEvent(
                            command.NewPassword
                        )
                    ));
                } else throw new DomainException("Failed to change the password");
            } else throw new DomainException("Invalid SignIn Method");
        }
    }
}