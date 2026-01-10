using System;
using System.Linq;

namespace Hermes.Core
{
    public static class ArticleTemplateEvents
    {
        public static void Apply(ArticleTemplate articleTemplate, DomainEvent<Guid, object> @event)
        {
            switch (@event.Payload) {
                case ArticleTemplateDeletedEvent e:
                    articleTemplate.Deleted = true;
                    break;
                case ArticleTemplateUploadedEvent e:
                    articleTemplate.ID = @event.Metadata.ID;
                    articleTemplate.LanguageID = e.LanguageID;
                    articleTemplate.TopicID = e.TopicID;
                    articleTemplate.Title = e.Title;
                    articleTemplate.Text = e.Text;
                    articleTemplate.Source = e.Source;
                    articleTemplate.PhotoURL = e.PhotoURL;
                    articleTemplate.Timestamp = @event.Metadata.Timestamp;
                    articleTemplate.Created = true;
                    break;
                case ArticleTemplateVideoUploadedEvent e:
                    articleTemplate.ID = @event.Metadata.ID;
                    articleTemplate.LanguageID = e.LanguageID;
                    articleTemplate.TopicID = e.TopicID;
                    articleTemplate.Title = e.Title.Select(s => s.Text).ToList();
                    articleTemplate.Text = e.Text.Select(s => s.Text).ToList();
                    articleTemplate.Source = e.Source;
                    articleTemplate.PhotoURL = e.PhotoURL;
                    articleTemplate.Timestamp = @event.Metadata.Timestamp;
                    articleTemplate.Created = true;
                    break;
            }
            articleTemplate.Version = @event.Metadata.Version;
        }
    }
}