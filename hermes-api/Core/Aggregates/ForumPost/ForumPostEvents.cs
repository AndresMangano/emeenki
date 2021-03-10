using System;
using System.Collections.Generic;

namespace Hermes.Core
{
    public static class ForumPostEvents
    {
        public static void Apply(ForumPost forumPost, DomainEvent<Guid, object> @event)
        {
            switch (@event.Payload)
            {
                case ForumPostCreatedEvent e:
                    forumPost.ID = @event.Metadata.ID;
                    forumPost.Created = true;
                    forumPost.Deleted = false;
                    forumPost.Title = e.Title;
                    forumPost.Text = e.Text;
                    forumPost.LanguageID = e.LanguageID;
                    forumPost.UserID = e.LanguageID;
                    forumPost.Timestamp = @event.Metadata.Timestamp;
                    forumPost.Comments = new List<ForumPostComment>();
                    break;
                case ForumPostEditedEvent e:
                    forumPost.Title = e.Title;
                    forumPost.Text = e.Text;
                    forumPost.LanguageID = e.LanguageID;
                    forumPost.ModifiedOn = @event.Metadata.Timestamp;
                    break;
                case ForumPostDeletedEvent:
                    forumPost.Deleted = true;
                    break;
                case ForumPostCommentedEvent e:
                    forumPost.Comments.Add(new ForumPostComment {
                        ID = e.ForumPostCommentID,
                        Text = e.Text,
                        UserID= e.UserID,
                        Timestamp = @event.Metadata.Timestamp
                    });
                    break;
                case ForumPostCommentDeletedEvent e:
                    forumPost.Comments
                        .RemoveAll(c => c.ID == e.ForumPostCommentID);
                    break;
            }
            forumPost.Version = @event.Metadata.Version;
        }
    }
}