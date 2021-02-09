ALTER TABLE Query_ArticleComments
DROP INDEX ArticleID, 
ADD UNIQUE KEY ArticleID (ArticleID, CommentIndex, ChildCommentIndex)