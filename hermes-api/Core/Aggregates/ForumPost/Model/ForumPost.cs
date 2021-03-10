using System;
using System.Collections.Generic;

namespace Hermes.Core
{
    public class ForumPost : IAggregateRoot<Guid>
    {
        public Guid ID { get; set; }
        public int Version { get; set; }
        public bool Created { get; set; }
        public bool Deleted { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string LanguageID { get; set; }
        public string UserID { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public List<ForumPostComment> Comments { get; set; }
    }
}