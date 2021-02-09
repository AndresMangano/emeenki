using System;
using System.Collections.Generic;

namespace Hermes.Core
{
    public class ArticleDTO
    {
        public Guid ArticleID { get; set; }
        public Guid ArticleTemplateID { get; set; }
        public bool Archived { get; set; }
        public string RoomID { get; set; }
        public string OriginalLanguageID { get; set; }
        public string TranslationLanguageID { get; set; }
        public IEnumerable<SentenceDTO> title { get; set; }
        public IEnumerable<SentenceDTO> text { get; set; }
        public string Source { get; set; }
        public string PhotoURL { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }
        public DateTime Timestamp { get; set; }

        public class SentenceDTO
        {
            public Guid ArticleID { get; set; }
            public bool InText { get; set; }
            public int SentenceIndex { get; set; }
            public string OriginalText { get; set; }
            public IEnumerable<TranslationDTO> TranslationHistory { get; set; }
        }

        public class TranslationDTO
        {
            public Guid ArticleID { get; set; }
            public bool InText { get; set; }
            public int SentenceIndex { get; set; }
            public int TranslationIndex { get; set; }
            public string Translation { get; set; }
            public string UserID { get; set; }
            public string ProfilePhotoURL { get; set; }
            public string NativeLanguageID { get; set; }
            public IEnumerable<string> Upvotes { get; set; }
            public IEnumerable<string> Downvotes { get; set; }
            public IEnumerable<TranslationCommentDTO> Comments { get; set; }
            public DateTime Timestamp { get; set; }
        }

        public class TranslationCommentDTO
        {
            public Guid ArticleID { get; set; }
            public bool InText { get; set; }
            public int SentenceIndex { get; set; }
            public int TranslationIndex { get; set; }
            public int CommentIndex { get; set; }
            public string Comment { get; set; }
            public string UserID { get; set; }
            public string ProfilePhotoURL { get; set; }
            public string NativeLanguageID { get; set; }
            public DateTime Timestamp { get; set; }
        }

        public class CommentDTO
        {
            public Guid ArticleID { get; set; }
            public int CommentIndex { get; set; }
            public int? ChildCommentIndex { get; set; }
            public string Comment { get; set; }
            public string UserID { get; set; }
            public string ProfilePhotoURL { get; set; }
            public string NativeLanguageID { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}