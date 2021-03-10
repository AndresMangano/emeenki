CREATE TABLE ForumPost_Events (
    Seq BIGINT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    ID CHAR(36) NOT NULL,
    Version INT NOT NULL,
    EventName VARCHAR(64) NOT NULL,
    `Timestamp` DATETIME NOT NULL,
    Payload TEXT NOT NULL,

    UNIQUE(ID, Version)
);

CREATE TABLE Query_ForumPosts (
    ID CHAR(36) NOT NULL PRIMARY KEY,
    Title TEXT NOT NULL,
    `Text` TEXT NOT NULL,
    LanguageID CHAR(3) NOT NULL,
    UserID VARCHAR(255) NOT NULL,
    `Timestamp` DATETIME NOT NULL,
    ModifiedOn DATETIME
);

CREATE TABLE Query_ForumPostComments (
    ID CHAR(36) NOT NULL PRIMARY KEY,
    ForumPostID CHAR(36) NOT NULL,
    `Text` TEXT NOT NULL,
    UserID VARCHAR(255) NOT NULL,
    `Timestamp` DATETIME NOT NULL
);