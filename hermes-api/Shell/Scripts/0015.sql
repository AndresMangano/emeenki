DROP TABLE hermes.EventHandlers;
CREATE TABLE Worker_Handlers (
    Stream VARCHAR(36) NOT NULL PRIMARY KEY,
    SeqID BIGINT NOT NULL
);