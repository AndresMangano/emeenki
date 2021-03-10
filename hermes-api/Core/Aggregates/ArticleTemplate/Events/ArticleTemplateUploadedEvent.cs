using System.Collections.Generic;

namespace Hermes.Core
{
    public record ArticleTemplateUploadedEvent(
        string LanguageID,
        List<string> Title,
        List<string> Text,
        string Source,
        string PhotoURL,
        string TopicID
    );
}