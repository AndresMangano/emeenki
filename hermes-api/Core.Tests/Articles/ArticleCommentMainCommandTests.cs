using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Xunit;

namespace Hermes.Core.Tests.Articles
{
    public class ArticleCommentMainCommandTests
    {
        readonly TestDomainInterpreter _context;

        public ArticleCommentMainCommandTests()
        {
            _context = new TestDomainInterpreter();
        }

        [Fact]
        void GivenAnExistingArticle_WhenCommentIt_ThenAddItToTheArticle()
        {
            // Arrange
            var articleID = Guid.NewGuid();
            _context.GivenAnArticle(articleID, "user1");
            // Act
            ArticleCommands.Execute(_context, new ArticleCommentMainCommand {
                ArticleID = articleID,
                Comment = "Some comment"
            }, "user1");
            // Assert
            var article = _context.Articles.Single(a => a.ID == articleID);
            var comment = article.Comments[0];
            Assert.Equal("Some comment", comment.Text);
            Assert.Equal("user1", comment.UserID);
            Assert.Equal(0, comment.Index);
            Assert.False(comment.Deleted);
            Assert.Empty(comment.Replies);
        }
        [Fact]
        void GivenAnArticleComment_WhenReplyToIt_ThenAddItIsAddedAsAChildComment()
        {
            // Arrange
            var articleID = Guid.NewGuid();
            _context.GivenAnArticle(articleID, "user2",
                withComments: new (string, string, IEnumerable<(string, string)>)[]{
                    ("Some comment", "user2", null),
                    ("Some other comment", "user2", null)
                }
            );
            // Act
            ArticleCommands.Execute(_context, new ArticleCommentMainCommand {
                ArticleID = articleID,
                Comment = "Child comment",
                ParentCommentPos = 1
            }, "user2");
            // Assert
            var article = _context.Articles.Single(a => a.ID == articleID);
            var reply = article.Comments[1].Replies[0];
            Assert.Equal("Child comment", reply.Text);
            Assert.Equal("user2", reply.UserID);
            Assert.Equal(0, reply.Index);
            Assert.False(reply.Deleted);
            Assert.Null(reply.Replies);
        }
        [Fact]
        void GivenAnArticle_WhenReplyToInvalidComment_ThenExceptionIsThrown()
        {
            // Arrange
            var articleID = Guid.NewGuid();
            _context.GivenAnArticle(articleID, "hox1",
                withComments: new (string, string, IEnumerable<(string, string)>)[]{
                    ("Some comment", "hox1", null)
                }
            );
            // Act
            var exception = Assert.Throws<DomainException>(() => {
                ArticleCommands.Execute(_context, new ArticleCommentMainCommand {
                    ArticleID = articleID,
                    Comment = "Some invalid comment",
                    ParentCommentPos = 1
                }, "hox1");
            });
            Assert.Equal("Invalid parent comment", exception.Message);
        }
    }
}