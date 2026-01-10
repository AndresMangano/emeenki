using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hermes.Core;
using Hermes.Shell.Write;
using MySql.Data.MySqlClient;

namespace Hermes.Shell.Read
{
    internal class ArticleTemplateSentenceQueryDTO
    {
        public bool InText { get; set; }
        public int SentenceIndex { get; set; }
        public string Sentence { get; set; }
        public long? StartTimeMs { get; set; }
        public long? EndTimeMs { get; set; }
    }

    public class ArticleTemplateQuery : IArticleTemplateQueries
    {
        private readonly string _connectionString;

        public ArticleTemplateQuery(SQLConnection connection)
        {
            _connectionString = connection.ConnectionString;
        }

        public async Task<ArticleTemplateDTO> Get(Guid articleTemplateID)
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)){
                conn.Open();
                var articleTemplate = await conn.QuerySingleAsync<ArticleTemplateDTO>("SELECT * FROM Query_ArticleTemplate WHERE ArticleTemplateID = @ArticleTemplateID",
                    new { ArticleTemplateID = articleTemplateID });
                var sentences = await conn.QueryAsync<ArticleTemplateSentenceQueryDTO>("SELECT * FROM Query_ArticleTemplateSentence WHERE ArticleTemplateID = @ArticleTemplateID",
                    new { ArticleTemplateID = articleTemplateID });

                articleTemplate.Title = sentences.Where(s => !s.InText).OrderBy(s => s.SentenceIndex)
                    .Select(s => new ArticleTemplateSentenceDTO 
                    { 
                        Text = s.Sentence, 
                        StartTimeMs = s.StartTimeMs, 
                        EndTimeMs = s.EndTimeMs 
                    });
                articleTemplate.Text = sentences.Where(s => s.InText).OrderBy(s => s.SentenceIndex)
                    .Select(s => new ArticleTemplateSentenceDTO 
                    { 
                        Text = s.Sentence, 
                        StartTimeMs = s.StartTimeMs, 
                        EndTimeMs = s.EndTimeMs 
                    });

                return articleTemplate;
            }
        }

        public async Task<IEnumerable<ArticleTemplatesDTO>> Query(string languageID, string topicID, bool archived)
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)){
                conn.Open();
                return await conn.QueryAsync<ArticleTemplatesDTO>(
                    @"  SELECT *
                            FROM Query_ArticleTemplates
                            WHERE   Archived = @archived AND
                                (@languageID IS NULL OR @languageID = LanguageID) AND
                                (@topicID IS NULL OR @topicID = TopicID)", 
                    new {
                        archived,
                        languageID,
                        topicID
                    }
                );
            }
        }
    }
}