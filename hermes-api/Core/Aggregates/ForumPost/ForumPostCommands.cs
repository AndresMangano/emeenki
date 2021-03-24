using System;
using System.Linq;
using Hermes.Core.Ports;

namespace Hermes.Core
{
    public static class ForumPostCommands
    {
        public static Guid Execute<IO>(IO io, ForumPostCreateCommand cmd, string userID)
            where IO: ILanguagesRepository, IEventsRepository
        {
            if (string.IsNullOrEmpty(cmd.Title))
                throw new DomainException("Forum post title cannot be empty");
            if (string.IsNullOrEmpty(cmd.Text))
                throw new DomainException("Forum post cannot be empty");

            var language = io.FetchLanguage(cmd.LanguageID);
            if (language == null)
                throw new DomainException("Invalid language");
            
            var forumPostID = Guid.NewGuid();
            io.StoreEvent(new ForumPostEvent(
                id: forumPostID,
                version: 1,
                eventName: "created",
                payload: new ForumPostCreatedEvent(
                    cmd.Title,
                    cmd.Text,
                    cmd.LanguageID,
                    userID
                )
            ));

            return forumPostID;
        }

        public static void Execute<IO>(IO io, ForumPostEditCommand cmd, string userID)
            where IO: ILanguagesRepository, IForumPostRepository, IEventsRepository
        {
            if (string.IsNullOrEmpty(cmd.Title))
                throw new DomainException("Forum post title cannot be empty");
            if (string.IsNullOrEmpty(cmd.Text))
                throw new DomainException("Forum post cannot be empty");
            
            var language = io.FetchLanguage(cmd.LanguageID);
            if (language == null)
                throw new DomainException("Invalid language");

            var forumPost = io.FetchForumPost(cmd.ForumPostID);
            if (forumPost == null)
                throw new DomainException("Forum post not found");

            io.StoreEvent(new ForumPostEvent(
                id: cmd.ForumPostID,
                version: forumPost.Version + 1,
                eventName: "edited",
                payload: new ForumPostEditedEvent(
                    cmd.Title,
                    cmd.Text,
                    cmd.LanguageID
                )
            ));
        }

        public static void Execute<IO>(IO io, ForumPostDeleteCommand cmd, string userID)
            where IO : IEventsRepository, IForumPostRepository
        {
            var forumPost = io.FetchForumPost(cmd.ForumPostID);
            if (forumPost == null)
                throw new DomainException("Forum post not found");
                
            if (!forumPost.Deleted)
                io.StoreEvent(new ForumPostEvent(
                    id: cmd.ForumPostID,
                    version: forumPost.Version + 1,
                    eventName: "deleted",
                    payload: new ForumPostDeletedEvent()
                ));
        }

        public static Guid Execute<IO>(IO io, ForumPostCommentCommand cmd, string userID)
            where IO : IEventsRepository, IForumPostRepository
        {
            if (string.IsNullOrEmpty(cmd.Text))
                throw new DomainException("Post comment cannot be empty");

            var forumPost = io.FetchForumPost(cmd.ForumPostID);
            if (forumPost == null)
                throw new DomainException("Forum post not found");

            var forumPostCommentID = Guid.NewGuid();
            io.StoreEvent(new ForumPostEvent(
                id: cmd.ForumPostID,
                version: forumPost.Version + 1,
                eventName: "commented",
                payload: new ForumPostCommentedEvent(
                    forumPostCommentID,
                    cmd.Text,
                    userID
                )
            ));

            return forumPostCommentID;
        }

        public static void Execute<IO>(IO io, ForumPostDeleteCommentCommand cmd, string userID)
            where IO : IEventsRepository, IForumPostRepository
        {
            var forumPost = io.FetchForumPost(cmd.ForumPostID);
            if (forumPost == null)
                throw new DomainException("Forum post not found");

            if (forumPost.Comments.Any(c => c.ID == cmd.ForumPostCommentID))
                io.StoreEvent(new ForumPostEvent(
                    id: cmd.ForumPostID,
                    version: forumPost.Version + 1,
                    eventName: "comment.deleted",
                    payload: new ForumPostCommentDeletedEvent(
                        cmd.ForumPostCommentID
                    )
                ));
        }
    }
}