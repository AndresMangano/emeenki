using System;
using Dapper;
using Hermes.Core;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Hermes.Shell.Services
{
    public class PublicRoomInitializer
    {
        private readonly string _connectionString;

        private const string PUBLIC_ADMIN_USER_ID = "publicadmin";

        // All language IDs available in the project
        private static readonly string[] LANGUAGE_IDS = new[]
        {
            "CHN", // Chinese
            "DUT", // Dutch
            "ENG", // English
            "FRE", // French
            "GER", // German
            "ITA", // Italian
            "JPN", // Japanese
            "KOR", // Korean
            "POR", // Portuguese
            "RUS", // Russian
            "SPA"  // Spanish
        };

        public PublicRoomInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Ensures that public rooms for ALL ordered language pairs exist.
        /// Example: PUBLIC_ENG_SPA and PUBLIC_SPA_ENG.
        /// </summary>
        public void EnsurePublicRoomExists()
        {
            try
            {
                // Make sure the publicadmin user exists once
                EnsurePublicAdminExists();

                // Create / fix all ordered language pairs
                foreach (var lang1 in LANGUAGE_IDS)
                {
                    foreach (var lang2 in LANGUAGE_IDS)
                    {
                        if (lang1 == lang2)
                            continue; // skip same-language pairs

                        var roomId = GetPublicRoomId(lang1, lang2);

                        if (RoomExists(roomId))
                        {
                            Console.WriteLine($"Public room {roomId} ({lang1}->{lang2}) already exists. Ensuring publicadmin has admin rights...");
                            EnsurePublicAdminHasAdminRights(roomId);
                        }
                        else
                        {
                            Console.WriteLine($"Creating public room {roomId} ({lang1}->{lang2}) with publicadmin as admin...");
                            CreatePublicRoom(roomId, lang1, lang2);
                            Console.WriteLine($"Public room {roomId} created successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error initializing PUBLIC rooms: {ex.Message}");
                Console.ResetColor();
                // Don't throw - we don't want to prevent app startup if this fails
            }
        }

        private static string GetPublicRoomId(string languageId1, string languageId2)
        {
            // Example: PUBLIC_ENG_SPA
            return $"PUBLIC_{languageId1}_{languageId2}";
        }

        private bool RoomExists(string roomID)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var count = conn.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM Room_Events WHERE ID = @ID AND EventName = 'opened'",
                    new { ID = roomID });
                return count > 0;
            }
        }

        private bool UserExists(string userID)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var count = conn.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM User_Events WHERE ID = @ID AND EventName = 'registered'",
                    new { ID = userID });
                return count > 0;
            }
        }

        private void EnsurePublicAdminExists()
        {
            if (UserExists(PUBLIC_ADMIN_USER_ID))
            {
                Console.WriteLine("publicadmin user already exists.");
                return;
            }

            Console.WriteLine("Creating publicadmin user...");

            // Create UserRegisteredEvent
            var userEvent = new
            {
                // Hardcoded password for publicadmin
                Password = "entrar",
                ProfilePhotoURL = "",
                LanguageID = "ENG",
                Rights = "admin",
                Country = ""
            };

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                conn.Execute(@"
                    INSERT INTO User_Events(ID, Version, EventName, `Timestamp`, Payload)
                    VALUES (@ID, @Version, @EventName, @Timestamp, @Payload);",
                    new
                    {
                        ID = PUBLIC_ADMIN_USER_ID,
                        Version = 1,
                        EventName = "registered",
                        Timestamp = DateTime.UtcNow,
                        Payload = JsonConvert.SerializeObject(userEvent)
                    });

                // Create query projection for the user
                conn.Execute(@"
                    INSERT INTO Query_User(UserID, Rights, ProfilePhotoURL, NativeLanguageID)
                    VALUES (@UserID, @Rights, @ProfilePhotoURL, @NativeLanguageID)
                    ON DUPLICATE KEY UPDATE Rights = @Rights;",
                    new
                    {
                        UserID = PUBLIC_ADMIN_USER_ID,
                        Rights = "admin",
                        ProfilePhotoURL = "",
                        NativeLanguageID = "ENG"
                    });
            }

            Console.WriteLine("publicadmin user created successfully.");
        }

        private void CreatePublicRoom(string roomId, string languageId1, string languageId2)
        {
            var roomOpenedEvent = new
            {
                Token = Guid.NewGuid(),
                LanguageID1 = languageId1,
                LanguageID2 = languageId2,
                UsersLimit = (short)100,
                Restricted = false,
                UserID = PUBLIC_ADMIN_USER_ID
            };

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                
                // Create the room opened event
                conn.Execute(@"
                    INSERT INTO Room_Events(ID, Version, EventName, `Timestamp`, Payload)
                    VALUES (@ID, @Version, @EventName, @Timestamp, @Payload);",
                    new
                    {
                        ID = roomId,
                        Version = 1,
                        EventName = "opened",
                        Timestamp = DateTime.UtcNow,
                        Payload = JsonConvert.SerializeObject(roomOpenedEvent)
                    });

                // Create query projections
                conn.Execute(@"
                    INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
                    VALUES (@RoomID, @LanguageID1, @LanguageID2, @Closed, @Restricted, @UsersLimit)
                    ON DUPLICATE KEY UPDATE Closed = @Closed;",
                    new
                    {
                        RoomID = roomId,
                        LanguageID1 = languageId1,
                        LanguageID2 = languageId2,
                        Closed = false,
                        Restricted = false,
                        UsersLimit = 100
                    });

                conn.Execute(@"
                    INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
                    VALUES (@RoomID, @UserID, @Permission)
                    ON DUPLICATE KEY UPDATE Permission = @Permission;",
                    new
                    {
                        RoomID = roomId,
                        UserID = PUBLIC_ADMIN_USER_ID,
                        Permission = "admin"
                    });
            }
        }

        private void EnsurePublicAdminHasAdminRights(string roomId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                
                // Check if publicadmin is in this room
                var existingPermission = conn.QueryFirstOrDefault<string>(@"
                    SELECT Permission FROM Query_RoomUser 
                    WHERE RoomID = @RoomID AND UserID = @UserID",
                    new
                    {
                        RoomID = roomId,
                        UserID = PUBLIC_ADMIN_USER_ID
                    });

                if (existingPermission == null)
                {
                    // Add publicadmin to room with admin rights
                    Console.WriteLine($"Adding publicadmin to {roomId}...");
                    conn.Execute(@"
                        INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
                        VALUES (@RoomID, @UserID, @Permission);",
                        new
                        {
                            RoomID = roomId,
                            UserID = PUBLIC_ADMIN_USER_ID,
                            Permission = "admin"
                        });
                }
                else if (existingPermission != "admin")
                {
                    // Update permission to admin
                    Console.WriteLine($"Updating publicadmin permission to admin in {roomId}...");
                    conn.Execute(@"
                        UPDATE Query_RoomUser SET Permission = @Permission
                        WHERE RoomID = @RoomID AND UserID = @UserID;",
                        new
                        {
                            Permission = "admin",
                            RoomID = roomId,
                            UserID = PUBLIC_ADMIN_USER_ID
                        });
                }
            }
        }

        private string GenerateRandomPassword()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

