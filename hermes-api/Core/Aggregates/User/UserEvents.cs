using System;
using System.Collections.Generic;
using System.Linq;

namespace Hermes.Core
{
    public static class UserEvents
    {
        public static void Apply(User user, DomainEvent<string, object> evnt)
        {
            switch (evnt.Payload) {
                case UserBannedEvent e: 
                    user.Banned = true;
                    break;
                case UserDeletedEvent e:
                    user.Deleted = true; 
                    break;
                case UserRegisteredEvent e: 
                    user.ID = evnt.Metadata.ID;
                    user.Banned = false;
                    user.SignInMethod = new PasswordSignIn {
                        Password = e.Password
                    };
                    user.ProfilePhotoURL = e.ProfilePhotoURL;
                    user.LanguageID = e.LanguageID;
                    user.Rights = UserService.ParseUserRights(e.Rights);
                    user.Country = e.Country;
                    user.Created = true;
                    user.Posts = new List<UserPost>();
                    break;
                case UserRegisteredWithGoogleEvent e:
                    user.ID = evnt.Metadata.ID;
                    user.Banned = false;
                    user.SignInMethod = new GoogleSignIn {
                        Email = e.GoogleEmail
                    };
                    user.ProfilePhotoURL = e.ProfilePhotoURL;
                    user.LanguageID = e.LanguageID;
                    user.Rights = UserService.ParseUserRights(e.Rights);
                    user.Country = e.Country;
                    user.Created = true;
                    user.Posts = new List<UserPost>();
                    break;
                case UserRightsChangedEvent e:
                    user.Rights = UserService.ParseUserRights(e.NewRights); 
                    break;
                case UserUnbannedEvent e:
                    user.Banned = false; 
                    break;
                case UserProfilePhotoChangedEvent e:
                    user.ProfilePhotoURL = e.ProfilePhotoURL;
                    break;
                case UserPostAddedEvent e:
                    if (e.ChildUserPostID == null) {
                        user.Posts.Add(new UserPost {
                            UserPostID = e.UserPostID,
                            Text = e.Text,
                            UserID = e.UserID,
                            Timestamp = evnt.Metadata.Timestamp,
                            Replies = new List<UserPost>()
                        });
                    } else {
                        var userPost = user.Posts.Single(p => p.UserPostID == e.UserPostID);
                        userPost.Replies.Add(new UserPost {
                            UserPostID = e.ChildUserPostID.Value,
                            Text = e.Text,
                            UserID = e.UserID,
                            Timestamp = evnt.Metadata.Timestamp
                        });
                    }
                    break;
                case UserPostDeletedEvent e:
                    if (e.ChildUserPostID == null) {
                        user.Posts.RemoveAll(p => p.UserPostID == e.UserPostID);
                    }
                    else {
                        user.Posts
                            .Single(p => p.UserPostID == e.UserPostID)
                            .Replies.RemoveAll(r => r.UserPostID == e.ChildUserPostID);
                    }
                    break;
                case UserLanguageChangedEvent e:
                    user.LanguageID = e.NativeLanguageID;
                    break;
                case UserDescriptionChangedEvent e:
                    user.Description = e.Description;
                    break;
                case UserCountryChangedEvent e:
                    user.Country = e.Country;
                    break;
                case UserPasswordChangedEvent e:
                    if (user.SignInMethod is PasswordSignIn passwordSignIn) {
                        passwordSignIn.Password = e.Password;
                    }
                    break;
            }
            user.Version = evnt.Metadata.Version;
        }
    }
}