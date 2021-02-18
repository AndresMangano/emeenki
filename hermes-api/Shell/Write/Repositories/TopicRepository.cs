using Dapper;
using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;
using Hermes.Core;
using Hermes.Core.Ports;
using Hermes.Shell.Write;

namespace Hermes.Shell
{
    public partial class DomainInterpreter : ITopicsRepository
    {
        public Topic FetchTopic(string topicID)
        {
            try {
                using (MySqlConnection conn = new MySqlConnection(_connection.ConnectionString)) {
                    var result = conn.QuerySingle<TopicModel>(@"
                        SELECT
                            T.TopicID,
                            T.`Name`
                        FROM Topics T
                        WHERE T.TopicID = @topicID
                        ORDER BY T.TopicID ASC",
                        new {
                            topicID
                        });
                    return new Topic {
                        ID = result.TopicID,
                        Version = 1,
                        Created = true,
                        Deleted = false,
                        Name = result.Name
                    };
                }
            } catch (Exception) {
                return new Topic {
                    ID = topicID,
                    Version = 0,
                    Created = false,
                    Deleted = false,
                    Name = null
                };
            }
        }
    }
}