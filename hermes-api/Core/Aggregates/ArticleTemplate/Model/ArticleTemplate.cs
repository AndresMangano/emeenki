using System;
using System.Collections.Generic;

namespace Hermes.Core
{
    public class ArticleTemplate : IAggregateRoot<Guid>
    {
        public Guid ID { get; set; }
        public int Version { get; set; }
        public bool Created { get; set; }
        public bool Deleted { get; set; }
        public string LanguageID { get; set; }
        public string TopicID { get; set; }
        public List<string> Title { get; set; }
        public List<string> Text { get; set; }
        public string Source { get; set; }
        public string PhotoURL { get; set; }
        public DateTime Timestamp { get; set; }
    }
}