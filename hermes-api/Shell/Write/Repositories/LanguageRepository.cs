using Dapper;
using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;
using Hermes.Core;
using Hermes.Core.Ports;
using Hermes.Shell.Write;

namespace Hermes.Shell
{
    public partial class DomainInterpreter : ILanguagesRepository
    {
        public Language FetchLanguage(string languageID)
        {
            var query = @"
                SELECT
                    L.LanguageID,
                    L.Description
                FROM Language_Language L
                WHERE L.LanguageId = @languageID
                ORDER BY L.LanguageID
            ";
            using (MySqlConnection conn = new MySqlConnection(_connection.ConnectionString)) {
                try{
                    var result = conn.QuerySingle<LanguageModel>(query, new {
                        languageID = languageID
                    });
                    return new Language {
                        ID = result.LanguageID,
                        Version = 1,
                        Created = true,
                        Deleted = false,
                        Description = result.Description
                    };
                } catch (Exception) {
                    return new Language {
                        ID = languageID,
                        Version = 0,
                        Created = false,
                        Deleted = false,
                        Description = null
                    };
                }
            }
        }
    }
}