using System;
using Hermes.Worker.Core.Model;
using Hermes.Worker.Core.Model.Events.User;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;
using Newtonsoft.Json;

namespace Hermes.Worker.Core.Commands
{
    public static class HandleUserEventCommand
    {
        public static void Execute<IO, dbIO>(IO io, string routingKey, string userEvent)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IUserRepository, IUserPostsRepository {
            try {
                switch (routingKey) {
                    case "registered": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, UserRegisteredEvent>>(userEvent).Message); break;
                    case "registered.withGoogle": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, UserRegisteredWithGoogleEvent>>(userEvent).Message); break;
                    case "deleted": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, UserDeletedEvent>>(userEvent).Message); break;
                    case "rights.changed": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, UserRightsChangedEvent>>(userEvent).Message); break;
                    case "profilePhotoChanged": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, UserProfilePhotoChangedEvent>>(userEvent).Message); break;
                    case "post.added": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, UserPostAddedEvent>>(userEvent).Message); break;
                    case "post.deleted": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, UserPostDeletedEvent>>(userEvent).Message); break;
                    case "language.changed": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, UserLanguageChangedEvent>>(userEvent).Message); break;
                    case "description.changed": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, UserDescriptionChangedEvent>>(userEvent).Message); break;
                    case "country.changed": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, UserCountryChangedEvent>>(userEvent).Message); break;
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error => {ex.Message}");
                throw;
            }
        }
        // Event Handlers
        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, UserRegisteredEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IUserRepository {
            io.Execute(dbIO => {
                var payload = message.Payload;
                dbIO.InsertUser(
                    userID: message.Metadata.ID,
                    rights: payload.Rights,
                    profilePhotoURL: payload.ProfilePhotoURL,
                    nativeLanguageID: payload.LanguageID,
                    country: payload.Country,
                    signInType: "password"
                );
            });
            io.SendSignalToGroup(SignalRSignal.USER_UPDATED, message.Metadata.ID, "users");
        }
        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, UserRegisteredWithGoogleEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IUserRepository {
            io.Execute(dbIO => {
                var payload = message.Payload;
                dbIO.InsertUser(
                    userID: message.Metadata.ID,
                    rights: payload.Rights,
                    profilePhotoURL: payload.ProfilePhotoURL,
                    nativeLanguageID: payload.LanguageID,
                    country: payload.Country,
                    signInType: "google"
                );
            });
            io.SendSignalToGroup(SignalRSignal.USER_UPDATED, message.Metadata.ID, "users");
        }
        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, UserDeletedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IUserRepository {
            io.Execute(dbIO => {
                dbIO.DeleteUser(message.Metadata.ID);
            });
            io.SendSignalToGroup(SignalRSignal.USER_UPDATED, message.Metadata.ID,
                "users",
                $"user:{message.Metadata.ID}");
        }
        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, UserLanguageChangedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IUserRepository {
            io.Execute(dbIO => {
                dbIO.UpdateUser(message.Metadata.ID,
                    nativeLanguageID: new DbUpdate<string>(message.Payload.NativeLanguageID));
            });
            io.SendSignalToGroup(SignalRSignal.USER_UPDATED, message.Metadata.ID,
                "users",
                $"user:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, UserDescriptionChangedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IUserRepository {
            io.Execute(dbIO => {
                dbIO.UpdateUser(message.Metadata.ID,
                    description: new DbUpdate<string>(message.Payload.Description));
            });
            io.SendSignalToGroup(SignalRSignal.USER_UPDATED, message.Metadata.ID,
                "users",
                $"user:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, UserCountryChangedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IUserRepository {
            io.Execute(dbIO => {
                dbIO.UpdateUser(message.Metadata.ID,
                    country: new DbUpdate<string>(message.Payload.Country));
            });
            io.SendSignalToGroup(SignalRSignal.USER_UPDATED, message.Metadata.ID,
                "users",
                $"user:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, UserPostAddedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IUserPostsRepository {
            io.Execute(dbIO => {
                var payload = message.Payload;
                dbIO.InsertUserPost(
                    userPostID: payload.UserPostID,
                    childUserPostID: payload.ChildUserPostID,
                    userID: message.Metadata.ID,
                    text: payload.Text,
                    senderUserID: payload.UserID,
                    timestamp: message.Metadata.Timestamp
                );
            });
            io.SendSignalToGroup(SignalRSignal.USER_UPDATED, message.Metadata.ID, $"user:{message.Metadata.ID}");
        }
        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, UserPostDeletedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IUserPostsRepository {
            io.Execute(dbIO => {
                dbIO.DeleteUserPost(
                    userPostID: message.Payload.UserPostID,
                    childUserPostID: message.Payload.ChildUserPostID
                );
            });
            io.SendSignalToGroup(SignalRSignal.USER_UPDATED, message.Metadata.ID, $"user:{message.Metadata.ID}");
        }
        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, UserProfilePhotoChangedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IUserRepository {
            io.Execute(dbIO => {
                dbIO.UpdateUser(message.Metadata.ID,
                    profilePhotoURL: new DbUpdate<string>(message.Payload.ProfilePhotoURL));
            });
            io.SendSignalToGroup(SignalRSignal.USER_UPDATED, message.Metadata.ID,
                "users",
                $"user:{message.Metadata.ID}");
        }
        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, UserRightsChangedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IUserRepository {
            io.Execute(dbIO => {
                dbIO.UpdateUser(message.Metadata.ID,
                    rights: new DbUpdate<string>(message.Payload.NewRights));
            });
            io.SendSignalToGroup(SignalRSignal.USER_UPDATED, message.Metadata.ID,
                "users",
                $"user:{message.Metadata.ID}");
        }
    }
}