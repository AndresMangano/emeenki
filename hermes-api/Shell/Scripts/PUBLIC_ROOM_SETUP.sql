INSERT IGNORE INTO User_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'root',
    1,
    'registered',
    NOW(),
    JSON_OBJECT(
        'Password', 'entrar',
        'ProfilePhotoURL', 'https://forums.crateentertainment.com/uploads/default/original/3X/c/e/ce92011c54c78c6ef5de3c22dadb061e7833ee2b.jpeg',
        'LanguageID', 'SPA',
        'Rights', 'admin',
        'Country', 'Argentina'
    )
);

INSERT IGNORE INTO User_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'publicadmin',
    1,
    'registered',
    NOW(),
    JSON_OBJECT(
        'Password', 'entrar',
        'ProfilePhotoURL', 'https://oyster.ignimgs.com/mediawiki/apis.ign.com/starcraft-2/8/81/Portrait_archon-large.jpg',
        'LanguageID', 'SPA',
        'Rights', 'admin',
        'Country', 'Argentina'
    )
);

INSERT INTO Query_User(UserID, Rights, ProfilePhotoURL, NativeLanguageID)
VALUES ('publicadmin', 'admin', '', 'ENG')
ON DUPLICATE KEY UPDATE Rights = 'admin';


INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_CHN_DUT',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'CHN',
        'LanguageID2', 'DUT',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_CHN_DUT', 'CHN', 'DUT', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_CHN_DUT', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_CHN_ENG',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'CHN',
        'LanguageID2', 'ENG',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_CHN_ENG', 'CHN', 'ENG', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_CHN_ENG', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_CHN_FRE',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'CHN',
        'LanguageID2', 'FRE',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_CHN_FRE', 'CHN', 'FRE', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_CHN_FRE', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_CHN_GER',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'CHN',
        'LanguageID2', 'GER',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_CHN_GER', 'CHN', 'GER', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_CHN_GER', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_CHN_ITA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'CHN',
        'LanguageID2', 'ITA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_CHN_ITA', 'CHN', 'ITA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_CHN_ITA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_CHN_JPN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'CHN',
        'LanguageID2', 'JPN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_CHN_JPN', 'CHN', 'JPN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_CHN_JPN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_CHN_KOR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'CHN',
        'LanguageID2', 'KOR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_CHN_KOR', 'CHN', 'KOR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_CHN_KOR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_CHN_POR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'CHN',
        'LanguageID2', 'POR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_CHN_POR', 'CHN', 'POR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_CHN_POR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_CHN_RUS',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'CHN',
        'LanguageID2', 'RUS',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_CHN_RUS', 'CHN', 'RUS', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_CHN_RUS', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_CHN_SPA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'CHN',
        'LanguageID2', 'SPA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_CHN_SPA', 'CHN', 'SPA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_CHN_SPA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';




--------------------------
-- DUT as LanguageID1
--------------------------

INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_DUT_CHN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'DUT',
        'LanguageID2', 'CHN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_DUT_CHN', 'DUT', 'CHN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_DUT_CHN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_DUT_ENG',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'DUT',
        'LanguageID2', 'ENG',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_DUT_ENG', 'DUT', 'ENG', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_DUT_ENG', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_DUT_FRE',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'DUT',
        'LanguageID2', 'FRE',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_DUT_FRE', 'DUT', 'FRE', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_DUT_FRE', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_DUT_GER',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'DUT',
        'LanguageID2', 'GER',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_DUT_GER', 'DUT', 'GER', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_DUT_GER', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_DUT_ITA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'DUT',
        'LanguageID2', 'ITA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_DUT_ITA', 'DUT', 'ITA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_DUT_ITA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_DUT_JPN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'DUT',
        'LanguageID2', 'JPN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_DUT_JPN', 'DUT', 'JPN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_DUT_JPN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_DUT_KOR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'DUT',
        'LanguageID2', 'KOR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_DUT_KOR', 'DUT', 'KOR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_DUT_KOR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_DUT_POR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'DUT',
        'LanguageID2', 'POR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_DUT_POR', 'DUT', 'POR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_DUT_POR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_DUT_RUS',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'DUT',
        'LanguageID2', 'RUS',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_DUT_RUS', 'DUT', 'RUS', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_DUT_RUS', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_DUT_SPA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'DUT',
        'LanguageID2', 'SPA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_DUT_SPA', 'DUT', 'SPA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_DUT_SPA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';




--------------------------
-- ENG as LanguageID1
--------------------------

INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ENG_CHN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ENG',
        'LanguageID2', 'CHN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ENG_CHN', 'ENG', 'CHN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ENG_CHN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ENG_DUT',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ENG',
        'LanguageID2', 'DUT',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ENG_DUT', 'ENG', 'DUT', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ENG_DUT', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ENG_FRE',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ENG',
        'LanguageID2', 'FRE',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ENG_FRE', 'ENG', 'FRE', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ENG_FRE', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ENG_GER',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ENG',
        'LanguageID2', 'GER',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ENG_GER', 'ENG', 'GER', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ENG_GER', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ENG_ITA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ENG',
        'LanguageID2', 'ITA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ENG_ITA', 'ENG', 'ITA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ENG_ITA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ENG_JPN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ENG',
        'LanguageID2', 'JPN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ENG_JPN', 'ENG', 'JPN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ENG_JPN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ENG_KOR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ENG',
        'LanguageID2', 'KOR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ENG_KOR', 'ENG', 'KOR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ENG_KOR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ENG_POR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ENG',
        'LanguageID2', 'POR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ENG_POR', 'ENG', 'POR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ENG_POR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ENG_RUS',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ENG',
        'LanguageID2', 'RUS',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ENG_RUS', 'ENG', 'RUS', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ENG_RUS', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ENG_SPA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ENG',
        'LanguageID2', 'SPA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ENG_SPA', 'ENG', 'SPA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ENG_SPA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';




--------------------------
-- FRE as LanguageID1
--------------------------

INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_FRE_CHN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'FRE',
        'LanguageID2', 'CHN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_FRE_CHN', 'FRE', 'CHN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_FRE_CHN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_FRE_DUT',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'FRE',
        'LanguageID2', 'DUT',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_FRE_DUT', 'FRE', 'DUT', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_FRE_DUT', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_FRE_ENG',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'FRE',
        'LanguageID2', 'ENG',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_FRE_ENG', 'FRE', 'ENG', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_FRE_ENG', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_FRE_GER',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'FRE',
        'LanguageID2', 'GER',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_FRE_GER', 'FRE', 'GER', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_FRE_GER', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_FRE_ITA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'FRE',
        'LanguageID2', 'ITA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_FRE_ITA', 'FRE', 'ITA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_FRE_ITA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_FRE_JPN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'FRE',
        'LanguageID2', 'JPN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_FRE_JPN', 'FRE', 'JPN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_FRE_JPN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_FRE_KOR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'FRE',
        'LanguageID2', 'KOR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_FRE_KOR', 'FRE', 'KOR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_FRE_KOR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_FRE_POR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'FRE',
        'LanguageID2', 'POR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_FRE_POR', 'FRE', 'POR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_FRE_POR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_FRE_RUS',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'FRE',
        'LanguageID2', 'RUS',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_FRE_RUS', 'FRE', 'RUS', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_FRE_RUS', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_FRE_SPA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'FRE',
        'LanguageID2', 'SPA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_FRE_SPA', 'FRE', 'SPA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_FRE_SPA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';




--------------------------
-- GER as LanguageID1
--------------------------

INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_GER_CHN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'GER',
        'LanguageID2', 'CHN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_GER_CHN', 'GER', 'CHN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_GER_CHN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_GER_DUT',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'GER',
        'LanguageID2', 'DUT',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_GER_DUT', 'GER', 'DUT', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_GER_DUT', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_GER_ENG',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'GER',
        'LanguageID2', 'ENG',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_GER_ENG', 'GER', 'ENG', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_GER_ENG', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_GER_FRE',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'GER',
        'LanguageID2', 'FRE',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_GER_FRE', 'GER', 'FRE', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_GER_FRE', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_GER_ITA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'GER',
        'LanguageID2', 'ITA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_GER_ITA', 'GER', 'ITA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_GER_ITA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_GER_JPN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'GER',
        'LanguageID2', 'JPN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_GER_JPN', 'GER', 'JPN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_GER_JPN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_GER_KOR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'GER',
        'LanguageID2', 'KOR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_GER_KOR', 'GER', 'KOR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_GER_KOR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_GER_POR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'GER',
        'LanguageID2', 'POR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_GER_POR', 'GER', 'POR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_GER_POR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_GER_RUS',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'GER',
        'LanguageID2', 'RUS',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_GER_RUS', 'GER', 'RUS', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_GER_RUS', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_GER_SPA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'GER',
        'LanguageID2', 'SPA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_GER_SPA', 'GER', 'SPA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_GER_SPA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';




--------------------------
-- ITA as LanguageID1
--------------------------

INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ITA_CHN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ITA',
        'LanguageID2', 'CHN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ITA_CHN', 'ITA', 'CHN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ITA_CHN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ITA_DUT',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ITA',
        'LanguageID2', 'DUT',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ITA_DUT', 'ITA', 'DUT', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ITA_DUT', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ITA_ENG',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ITA',
        'LanguageID2', 'ENG',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ITA_ENG', 'ITA', 'ENG', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ITA_ENG', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ITA_FRE',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ITA',
        'LanguageID2', 'FRE',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ITA_FRE', 'ITA', 'FRE', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ITA_FRE', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ITA_GER',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ITA',
        'LanguageID2', 'GER',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ITA_GER', 'ITA', 'GER', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ITA_GER', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ITA_JPN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ITA',
        'LanguageID2', 'JPN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ITA_JPN', 'ITA', 'JPN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ITA_JPN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ITA_KOR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ITA',
        'LanguageID2', 'KOR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ITA_KOR', 'ITA', 'KOR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ITA_KOR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ITA_POR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ITA',
        'LanguageID2', 'POR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ITA_POR', 'ITA', 'POR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ITA_POR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ITA_RUS',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ITA',
        'LanguageID2', 'RUS',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ITA_RUS', 'ITA', 'RUS', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ITA_RUS', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_ITA_SPA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'ITA',
        'LanguageID2', 'SPA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_ITA_SPA', 'ITA', 'SPA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_ITA_SPA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';




--------------------------
-- JPN as LanguageID1
--------------------------

INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_JPN_CHN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'JPN',
        'LanguageID2', 'CHN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_JPN_CHN', 'JPN', 'CHN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_JPN_CHN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_JPN_DUT',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'JPN',
        'LanguageID2', 'DUT',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_JPN_DUT', 'JPN', 'DUT', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_JPN_DUT', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_JPN_ENG',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'JPN',
        'LanguageID2', 'ENG',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_JPN_ENG', 'JPN', 'ENG', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_JPN_ENG', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_JPN_FRE',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'JPN',
        'LanguageID2', 'FRE',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_JPN_FRE', 'JPN', 'FRE', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_JPN_FRE', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_JPN_GER',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'JPN',
        'LanguageID2', 'GER',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_JPN_GER', 'JPN', 'GER', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_JPN_GER', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_JPN_ITA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'JPN',
        'LanguageID2', 'ITA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_JPN_ITA', 'JPN', 'ITA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_JPN_ITA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_JPN_KOR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'JPN',
        'LanguageID2', 'KOR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_JPN_KOR', 'JPN', 'KOR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_JPN_KOR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_JPN_POR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'JPN',
        'LanguageID2', 'POR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_JPN_POR', 'JPN', 'POR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_JPN_POR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_JPN_RUS',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'JPN',
        'LanguageID2', 'RUS',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_JPN_RUS', 'JPN', 'RUS', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_JPN_RUS', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_JPN_SPA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'JPN',
        'LanguageID2', 'SPA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_JPN_SPA', 'JPN', 'SPA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_JPN_SPA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';




--------------------------
-- KOR as LanguageID1
--------------------------

INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_KOR_CHN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'KOR',
        'LanguageID2', 'CHN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_KOR_CHN', 'KOR', 'CHN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_KOR_CHN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_KOR_DUT',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'KOR',
        'LanguageID2', 'DUT',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_KOR_DUT', 'KOR', 'DUT', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_KOR_DUT', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_KOR_ENG',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'KOR',
        'LanguageID2', 'ENG',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_KOR_ENG', 'KOR', 'ENG', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_KOR_ENG', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_KOR_FRE',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'KOR',
        'LanguageID2', 'FRE',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_KOR_FRE', 'KOR', 'FRE', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_KOR_FRE', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_KOR_GER',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'KOR',
        'LanguageID2', 'GER',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_KOR_GER', 'KOR', 'GER', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_KOR_GER', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_KOR_ITA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'KOR',
        'LanguageID2', 'ITA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_KOR_ITA', 'KOR', 'ITA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_KOR_ITA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_KOR_JPN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'KOR',
        'LanguageID2', 'JPN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_KOR_JPN', 'KOR', 'JPN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_KOR_JPN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_KOR_POR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'KOR',
        'LanguageID2', 'POR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_KOR_POR', 'KOR', 'POR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_KOR_POR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_KOR_RUS',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'KOR',
        'LanguageID2', 'RUS',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_KOR_RUS', 'KOR', 'RUS', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_KOR_RUS', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_KOR_SPA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'KOR',
        'LanguageID2', 'SPA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_KOR_SPA', 'KOR', 'SPA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_KOR_SPA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';




--------------------------
-- POR as LanguageID1
--------------------------

INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_POR_CHN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'POR',
        'LanguageID2', 'CHN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_POR_CHN', 'POR', 'CHN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_POR_CHN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_POR_DUT',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'POR',
        'LanguageID2', 'DUT',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_POR_DUT', 'POR', 'DUT', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_POR_DUT', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_POR_ENG',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'POR',
        'LanguageID2', 'ENG',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_POR_ENG', 'POR', 'ENG', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_POR_ENG', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_POR_FRE',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'POR',
        'LanguageID2', 'FRE',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_POR_FRE', 'POR', 'FRE', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_POR_FRE', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_POR_GER',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'POR',
        'LanguageID2', 'GER',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_POR_GER', 'POR', 'GER', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_POR_GER', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_POR_ITA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'POR',
        'LanguageID2', 'ITA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_POR_ITA', 'POR', 'ITA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_POR_ITA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_POR_JPN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'POR',
        'LanguageID2', 'JPN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_POR_JPN', 'POR', 'JPN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_POR_JPN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_POR_KOR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'POR',
        'LanguageID2', 'KOR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_POR_KOR', 'POR', 'KOR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_POR_KOR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_POR_RUS',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'POR',
        'LanguageID2', 'RUS',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_POR_RUS', 'POR', 'RUS', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_POR_RUS', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_POR_SPA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'POR',
        'LanguageID2', 'SPA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_POR_SPA', 'POR', 'SPA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_POR_SPA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';




--------------------------
-- RUS as LanguageID1
--------------------------

INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_RUS_CHN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'RUS',
        'LanguageID2', 'CHN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_RUS_CHN', 'RUS', 'CHN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_RUS_CHN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_RUS_DUT',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'RUS',
        'LanguageID2', 'DUT',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_RUS_DUT', 'RUS', 'DUT', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_RUS_DUT', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_RUS_ENG',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'RUS',
        'LanguageID2', 'ENG',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_RUS_ENG', 'RUS', 'ENG', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_RUS_ENG', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_RUS_FRE',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'RUS',
        'LanguageID2', 'FRE',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_RUS_FRE', 'RUS', 'FRE', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_RUS_FRE', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_RUS_GER',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'RUS',
        'LanguageID2', 'GER',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_RUS_GER', 'RUS', 'GER', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_RUS_GER', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_RUS_ITA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'RUS',
        'LanguageID2', 'ITA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_RUS_ITA', 'RUS', 'ITA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_RUS_ITA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_RUS_JPN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'RUS',
        'LanguageID2', 'JPN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_RUS_JPN', 'RUS', 'JPN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_RUS_JPN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_RUS_KOR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'RUS',
        'LanguageID2', 'KOR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_RUS_KOR', 'RUS', 'KOR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_RUS_KOR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_RUS_POR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'RUS',
        'LanguageID2', 'POR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_RUS_POR', 'RUS', 'POR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_RUS_POR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_RUS_SPA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'RUS',
        'LanguageID2', 'SPA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_RUS_SPA', 'RUS', 'SPA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_RUS_SPA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';




--------------------------
-- SPA as LanguageID1
--------------------------

INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_SPA_CHN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'SPA',
        'LanguageID2', 'CHN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_SPA_CHN', 'SPA', 'CHN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_SPA_CHN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_SPA_DUT',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'SPA',
        'LanguageID2', 'DUT',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_SPA_DUT', 'SPA', 'DUT', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_SPA_DUT', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_SPA_ENG',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'SPA',
        'LanguageID2', 'ENG',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_SPA_ENG', 'SPA', 'ENG', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_SPA_ENG', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_SPA_FRE',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'SPA',
        'LanguageID2', 'FRE',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_SPA_FRE', 'SPA', 'FRE', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_SPA_FRE', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_SPA_GER',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'SPA',
        'LanguageID2', 'GER',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_SPA_GER', 'SPA', 'GER', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_SPA_GER', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_SPA_ITA',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'SPA',
        'LanguageID2', 'ITA',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_SPA_ITA', 'SPA', 'ITA', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_SPA_ITA', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_SPA_JPN',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'SPA',
        'LanguageID2', 'JPN',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_SPA_JPN', 'SPA', 'JPN', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_SPA_JPN', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_SPA_KOR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'SPA',
        'LanguageID2', 'KOR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_SPA_KOR', 'SPA', 'KOR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_SPA_KOR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_SPA_POR',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'SPA',
        'LanguageID2', 'POR',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_SPA_POR', 'SPA', 'POR', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_SPA_POR', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';



INSERT IGNORE INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
VALUES (
    'PUBLIC_SPA_RUS',
    1,
    'opened',
    NOW(),
    JSON_OBJECT(
        'Token', UUID(),
        'LanguageID1', 'SPA',
        'LanguageID2', 'RUS',
        'UsersLimit', 100,
        'Restricted', false,
        'UserID', 'publicadmin'
    )
);

INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
VALUES ('PUBLIC_SPA_RUS', 'SPA', 'RUS', 0, 0, 100)
ON DUPLICATE KEY UPDATE Closed = 0;

INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
VALUES ('PUBLIC_SPA_RUS', 'publicadmin', 'admin')
ON DUPLICATE KEY UPDATE Permission = 'admin';

