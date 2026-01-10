-- Add video support to article templates
ALTER TABLE Query_ArticleTemplate
    ADD COLUMN IsVideo BIT NOT NULL DEFAULT 0,
    ADD COLUMN VideoURL VARCHAR(255) NULL;

-- Add timestamps to sentences for video caption synchronization
ALTER TABLE Query_ArticleTemplateSentence
ADD COLUMN StartTimeMs BIGINT NULL,
ADD COLUMN EndTimeMs BIGINT NULL;

-- Add video support to articles
ALTER TABLE Query_Article
ADD COLUMN IsVideo BIT NOT NULL DEFAULT 0,
ADD COLUMN VideoURL VARCHAR(255) NULL,
ADD COLUMN TopicID VARCHAR(50) NULL;

-- Add timestamps to article sentences for video caption synchronization
ALTER TABLE Query_Sentence
ADD COLUMN StartTimeMs BIGINT NULL,
ADD COLUMN EndTimeMs BIGINT NULL;




