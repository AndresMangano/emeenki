DROP TABLE hermes.Query_UserPosts;

CREATE TABLE Query_UserPosts(
    ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    UserPostID CHAR(36) NOT NULL,
    ChildUserPostID CHAR(36) NOT NULL,
	UserID VARCHAR(255) NOT NULL,
    `Text` TEXT NOT NULL,
    SenderUserID VARCHAR(255) NOT NULL,
	`Timestamp` DATETIME NOT NULL,

    UNIQUE(UserPostID, ChildUserPostID)
);

TRUNCATE TABLE hermes.Worker_Handlers;