CREATE TABLE Topics (
    TopicID CHAR(3) NOT NULL PRIMARY KEY,
    `Name` VARCHAR(255) NOT NULL
);

INSERT INTO Topics(TopicID, `Name`)
VALUES
    ('OTH', 'Other'),
    ('SCI', 'Science'),
    ('TEC', 'Technology'),
    ('POL', 'Politics'),
    ('ECO', 'Economy'),
    ('BUS', 'Business'),
    ('CUL', 'Culture'),
    ('ENT', 'Entertainment'),
    ('HIS', 'History'),
    ('DIS', 'Disaster'),
    ('CON', 'Conflict'),
    ('STT', 'Storytelling'),
    ('HLT', 'Health'),
    ('WIK', 'Wikipedia');

ALTER TABLE Query_ArticleTemplate
ADD TopicID CHAR(3);

ALTER TABLE Query_ArticleTemplates
ADD TopicID CHAR(3);