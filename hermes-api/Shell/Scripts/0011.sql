ALTER TABLE Query_UserPosts
ADD ChildUserPostID CHAR(36);

ALTER TABLE Query_UserPosts
DROP INDEX UserPostID, 
ADD UNIQUE KEY UserPostID (UserPostID, ChildUserPostID);