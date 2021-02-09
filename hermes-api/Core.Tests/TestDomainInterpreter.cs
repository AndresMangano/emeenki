using System;
using System.Collections.Generic;
using System.Linq;
using Hermes.Core.Ports;

namespace Hermes.Core.Tests
{
    public partial class TestDomainInterpreter
        : IEventsRepository, IArticlesRepository, IArticleTemplatesRepository, IRoomsRepository, IUsersRepository, ILanguagesRepository
    {
        public List<Article> Articles { get; }
        public List<ArticleTemplate> ArticleTemplates { get; }
        public List<Language> Languages { get; }
        public List<Room> Rooms {get; }
        public List<User> Users { get; }

        public TestDomainInterpreter()
        {
            Articles = new List<Article>();
            ArticleTemplates = new List<ArticleTemplate>();
            Languages = new List<Language>{
                new Language { ID = "ENG", Description = "English", Version = 1, Created = true, Deleted = false },
                new Language { ID = "SPA", Description = "Spanish", Version = 1, Created = true, Deleted = false },
                new Language { ID = "RUS", Description = "Russian", Version = 1, Created = true, Deleted = false },
                new Language { ID = "ITA", Description = "Italian", Version = 1, Created = true, Deleted = false },
                new Language { ID = "FRE", Description = "French", Version = 1, Created = true, Deleted = false },
                new Language { ID = "GER", Description = "German", Version = 1, Created = true, Deleted = false },
                new Language { ID = "POR", Description = "Portuguese", Version = 1, Created = true, Deleted = false },
                new Language { ID = "DUT", Description = "Dutch", Version = 1, Created = true, Deleted = false }
            };
            Rooms = new List<Room>();
            Users = new List<User>();
        }

        #region Fakes
        public Article FetchArticle(Guid articleID)
            => Articles.SingleOrDefault(a => a.ID == articleID) ?? new Article();

        public ArticleTemplate FetchArticleTemplate(Guid articleTemplateID)
            => ArticleTemplates.SingleOrDefault(at => at.ID == articleTemplateID) ?? new ArticleTemplate();

        public Language FetchLanguage(string languageID)
            => Languages.Single(l => l.ID == languageID);

        public Room FetchRoom(string roomID)
            => Rooms.SingleOrDefault(r => r.ID == roomID) ?? new Room();

        public User FetchUser(string userID)
            => Users.SingleOrDefault(u => u.ID == userID) ?? new User();

        public void StoreEvent(IDomainEvent evnt)
        {
            switch(evnt)
            {
                case ArticleEvent e:
                    var article = FetchArticle(e.Metadata.ID);
                    var articleCreated = article.Created;
                    ArticleEvents.Apply(article, e);
                    if (!articleCreated) Articles.Add(article);
                    break;
                case ArticleTemplateEvent e:
                    var articleTemplate = FetchArticleTemplate(e.Metadata.ID);
                    var articleTemplateCreated = articleTemplate.Created;
                    ArticleTemplateEvents.Apply(articleTemplate, e);
                    if (!articleTemplateCreated) ArticleTemplates.Add(articleTemplate);
                    break;
                case RoomEvent e:
                    var room = FetchRoom(e.Metadata.ID);
                    var roomCreated = room.Created;
                    RoomEvents.Apply(room, e);
                    if (!roomCreated) Rooms.Add(room);
                    break;
                case UserEvent e:
                    var user = FetchUser(e.Metadata.ID);
                    var userCreated = user.Created;
                    UserEvents.Apply(user, e);
                    if (!userCreated) Users.Add(user);
                    break;
                default:
                    throw new DomainException("Invalid event");
            }
        }
        #endregion

        #region Preconditions
        public void GivenARoom(string roomID, string userID = "hoxon")
        {
            Rooms.Add(new Room {
                ID = roomID,
                Version = 1,
                Created = true,
                Deleted = false,
                Languages = new string[]{ "SPA", "ENG" },
                UsersLimit = 10,
                Users = new List<RoomUser> {
                    new RoomUser {
                        UserID = userID,
                        Permission = RoomPermission.ADMIN
                    }
                },
                UsersQueue = new HashSet<string>(),
                Restricted = false,
                Token = new RoomToken {
                    Token = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow
                }
            });
        }
        public void GivenAnArticle(Guid articleID, string userID, string roomID = "someroom",
            IEnumerable<(string text, string userID, IEnumerable<(string text, string userID)> withChildComments)> withComments = null)
        {
            GivenARoom(roomID, userID);
            Articles.Add(new Article {
                ID = articleID,
                Version = 1,
                Created = true,
                Deleted = false,
                ArticleTemplateID = Guid.NewGuid(),
                RoomID = roomID,
                OriginalLanguageID = "SPA",
                TranslationLanguageID = "ENG",
                Title = new List<Sentence> {
                    new Sentence {
                        Index = 0,
                        OriginalText = "Some title",
                        TranslationHistory = new List<Translation>()
                    }
                },
                Text = new List<Sentence> {
                    new Sentence {
                        Index = 0,
                        OriginalText = "Some text",
                        TranslationHistory = new List<Translation>()
                    }
                },
                Source = "http://source.com",
                PhotoURL = "http://source.png",
                Timestamp = DateTime.UtcNow,
                Comments = withComments == null
                    ? new List<Comment>()
                    : withComments.Select((comment, i) => new Comment {
                        Index = i,
                        Text = comment.text,
                        UserID = comment.userID,
                        Deleted = false,
                        Timestamp = DateTime.UtcNow,
                        Replies = comment.withChildComments == null
                            ? new List<Comment>()
                            : comment.withChildComments.Select((child, ci) => new Comment {
                                Index = ci,
                                Text = child.text,
                                UserID = child.userID,
                                Deleted = false,
                                Timestamp = DateTime.UtcNow,
                                Replies = null
                            }).ToList()
                    }).ToList()
            });
        }
        #endregion
    }
}