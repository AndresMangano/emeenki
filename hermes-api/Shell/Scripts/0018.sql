ALTER TABLE Query_ForumPosts
ADD COLUMN LastCommentUserID VARCHAR(255);

ALTER TABLE Query_ForumPosts
ADD COLUMN LastCommentTimestamp DATETIME;