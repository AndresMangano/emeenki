using System;
using System.Collections.Generic;
using System.Linq;

namespace Hermes.Core
{
    public static class ArticleEvents
    {
        public static void Apply(Article article, DomainEvent<Guid, object> evnt)
        {
            switch (evnt.Payload) {
                case ArticleArchivedEvent e:
                    article.Deleted = true;
                    break;
                case ArticleCommentedEvent e: 
                    ArticleService.GetTranslation(article, e.InText, e.SentencePos, e.TranslationPos).Comments
                        .Add(new TranslationComment {
                            Index = e.CommentPos,
                            Comment = e.Comment,
                            UserID = e.UserID,
                            Timestamp = evnt.Metadata.Timestamp
                        });
                    break;
                case ArticleMainCommentedEvent e:
                    if (e.ChildCommentPos == null) {
                        article.Comments.Add(new Comment {
                            Index = e.CommentPos,
                            Text = e.Comment,
                            UserID = e.UserID,
                            Deleted = false,
                            Timestamp = evnt.Metadata.Timestamp,
                            Replies = new List<Comment>()
                        });
                    } else {
                        article.Comments[e.CommentPos].Replies.Add(new Comment {
                            Index = e.ChildCommentPos.Value,
                            Text = e.Comment,
                            UserID = e.UserID,
                            Deleted = false,
                            Timestamp = evnt.Metadata.Timestamp
                        });
                    }
                    break;
                case ArticleDownVotedEvent e:
                    Translation advet = ArticleService.GetTranslation(article, e.InText, e.SentencePos, e.TranslationPos);
                    advet.Upvotes.Remove(e.UserID);
                    advet.Downvotes.Add(e.UserID);
                    break;
                case ArticleDownVoteRemovedEvent e:
                    ArticleService.GetTranslation(article, e.InText, e.SentencePos, e.TranslationPos).Downvotes
                        .Remove(e.UserID);
                    break;
                case ArticleTemplateTakenEvent e:
                    article.ID = evnt.Metadata.ID;
                    article.ArticleTemplateID = e.ArticleTemplateID;
                    article.RoomID = e.RoomID;
                    article.OriginalLanguageID = e.OriginalLanguageID;
                    article.TranslationLanguageID = e.TranslationLanguageID;
                    article.Title = e.Title.Select((s, index) => new Sentence {
                        Index = index,
                        OriginalText = s,
                        TranslationHistory = new List<Translation>()
                    }).ToList();
                    article.Text = e.Text.Select((s, index) => new Sentence {
                        Index = index,
                        OriginalText = s,
                        TranslationHistory = new List<Translation>()
                    }).ToList();
                    article.Source = e.Source;
                    article.PhotoURL = e.PhotoURL;
                    article.Timestamp = evnt.Metadata.Timestamp;
                    article.Comments = new List<Comment>();
                    article.Created = true;
                    break;
                case ArticleTranslatedEvent e:
                    Sentence ates = ArticleService.GetSentence(article, e.InText, e.SentencePos);
                    if(e.TranslationPos < ates.TranslationHistory.Count){
                        var lastTranslation = ates.TranslationHistory[e.TranslationPos];
                        lastTranslation.Text = e.Translation;
                        lastTranslation.Timestamp = evnt.Metadata.Timestamp;
                        lastTranslation.UserID = e.UserID;
                    } else {
                        ates.TranslationHistory.Add(new Translation {
                            Index = e.TranslationPos,
                            Text = e.Translation,
                            UserID = e.UserID,
                            Timestamp = evnt.Metadata.Timestamp,
                            Upvotes = new HashSet<string>(),
                            Downvotes = new HashSet<string>(),
                            Comments = new List<TranslationComment>()
                        });
                    }
                    break;
                case ArticleUpVotedEvent e:
                    Translation auvet = ArticleService.GetTranslation(article, e.InText, e.SentencePos, e.TranslationPos);
                    auvet.Upvotes.Add(e.UserID);
                    auvet.Downvotes.Remove(e.UserID);
                    break;
                case ArticleUpVoteRemovedEvent e:
                    Translation auvret = ArticleService.GetTranslation(article, e.InText, e.SentencePos, e.TranslationPos);
                    auvret.Upvotes.Remove(e.UserID);
                    break;
                case ArticleMainCommentDeletedEvent e:
                    Comment comment = article.Comments.Find(c => c.Index == e.CommentPos);
                    if (e.ChildCommentPos != null) {
                        comment = comment.Replies.Single(r => r.Index == e.ChildCommentPos);
                    }
                    comment.Deleted = true;
                    break;
            }
            article.Version = evnt.Metadata.Version;
        }
    }
}