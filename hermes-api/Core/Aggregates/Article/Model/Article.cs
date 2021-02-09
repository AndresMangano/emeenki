using System;
using System.Collections.Generic;

namespace Hermes.Core
{
    public class Article : IAggregateRoot<Guid>
    {
        public Guid ID { get; set; }
        public int Version { get; set; }
        public bool Created { get; set; }
        public bool Deleted { get; set; }
        public Guid ArticleTemplateID { get; set; }
        public string RoomID { get; set; }
        public string OriginalLanguageID { get; set; }
        public string TranslationLanguageID { get; set; }
        public List<Sentence> Title { get; set; }
        public List<Sentence> Text { get; set; }
        public string Source { get; set; }
        public string PhotoURL { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Comment> Comments { get; set; }
    }
}