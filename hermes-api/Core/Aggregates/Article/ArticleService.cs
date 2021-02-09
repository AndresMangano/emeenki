using System.Collections.Generic;
using System.Linq;

namespace Hermes.Core
{
    public static class ArticleService
    {
        public static void ValidateExistence(Article article) {
            if (!article.Created || article.Deleted)
                throw new DomainException("The Article does not exist");
        }
        public static void ValidateDifferentTranslation(Article article, bool inText, int sentencePos, string translation) {
            Translation lastTranslation = ArticleService.GetLastTranslation(article, inText, sentencePos);

            if (lastTranslation != null && translation == lastTranslation.Text)
                throw new DomainException("The translation is the same as the last one");
        }
        public static void ValidateOwnComment(Article article, int commentPos, int? childCommentPos, string userId)
        {
            var comment = article.Comments.SingleOrDefault(c => c.Index == commentPos);
            if (childCommentPos != null) {
                comment = comment.Replies.SingleOrDefault(c => c.Index == childCommentPos);
            }
            if(comment == null)
                throw new DomainException("The comment does not exist");
            else if (comment.UserID != userId)
                throw new DomainException("You cannot delete a comment from another user");
        }
        public static Translation GetTranslation(Article article, bool inText, int sentencePos, int translationPos) {
            Sentence sentence = GetSentence(article, inText, sentencePos);
            Translation translation = sentence.TranslationHistory.Find((t) => t.Index == translationPos);
            
            return translation;
        }

        public static Sentence GetSentence(Article article, bool inText, int sentencePos) {
            List<Sentence> section = inText ? article.Text : article.Title;
            Sentence sentence = section.Find((s) => s.Index == sentencePos);
            
            return sentence;
        }

        public static Translation GetLastTranslation(Article article, bool inText, int sentencePos) {
            Sentence sentence = GetSentence(article, inText, sentencePos);
            var translationsCount = sentence.TranslationHistory.Count;
            if(translationsCount > 0)
                return sentence.TranslationHistory[translationsCount - 1];
            else
                return null;
        }

        public static bool ShouldReplaceLastTranslation(Article article, bool inText, int sentencePos, string userID)
        {
            Translation lastTranslation = GetLastTranslation(article, inText, sentencePos);

            if(lastTranslation != null && lastTranslation.UserID == userID){
                if( lastTranslation.Comments.Count > 0 ||
                    lastTranslation.Upvotes.Count > 0 ||
                    lastTranslation.Downvotes.Count > 0) {
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }
    }
}