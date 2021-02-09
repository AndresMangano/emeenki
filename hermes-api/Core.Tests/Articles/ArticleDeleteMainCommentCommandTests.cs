using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Hermes.Core.Tests.Articles
{
    public class ArticleDeleteMainCommentCommandTests
    {
        readonly TestDomainInterpreter _context;

        public ArticleDeleteMainCommentCommandTests()
        {
            _context = new TestDomainInterpreter();
        }

        [Fact]
        void GivenAnArticleChildComment_WhenDeleteIt_ThenMarkAsDeleted()
        {
            // Arrange
            var articleID = Guid.NewGuid();
            _context.GivenAnArticle(articleID, "hoxon",
                withComments: new (string, string, IEnumerable<(string, string)>)[] {
                    ("some comment", "hoxon", null),
                    ("some other comment", "hoxon", new (string, string)[]{
                        ("child comment", "hoxon"),
                        ("second child comment", "hoxon")
                    })
                }
            );
            // Act
            ArticleCommands.Execute(_context, new ArticleDeleteMainCommentCommand {
                ArticleID = articleID,
                CommentPos = 1,
                ChildCommentPos = 1
            }, "hoxon");
            // Assert
            var childComment =  (from article in _context.Articles
                                from comments in article.Comments
                                from childComments in comments.Replies
                                where article.ID == articleID && comments.Index == 1 && childComments.Index == 1
                                select childComments).Single();
            Assert.True(childComment.Deleted);
        }
        [Fact]
        void GivenAnArticleChildCommentFromAnotherUser_WhenDeleteIt_ThenThrowException()
        {
            // Arrange
            var articleID = Guid.NewGuid();
            _context.GivenAnArticle(articleID, "hoxon",
                withComments: new (string, string, IEnumerable<(string, string)>)[] {
                    ("some comment", "hoxon", null),
                    ("some other comment", "hoxon", new (string, string)[]{
                        ("child comment", "hoxon"),
                        ("second child comment", "someotheruser")
                    })
                }
            );
            // Act
            var exception = Assert.Throws<DomainException>(() => {
                ArticleCommands.Execute(_context, new ArticleDeleteMainCommentCommand {
                    ArticleID = articleID,
                    CommentPos = 1,
                    ChildCommentPos = 1
                }, "hoxon");
            });
            // Assert
            var childComment =  (from article in _context.Articles
                                from comments in article.Comments
                                from childComments in comments.Replies
                                where article.ID == articleID && comments.Index == 1 && childComments.Index == 1
                                select childComments).Single();
            Assert.False(childComment.Deleted);
            Assert.Equal("You cannot delete a comment from another user", exception.Message);
        }
    }
}