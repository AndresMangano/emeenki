using System;
using System.Collections.Generic;

namespace Hermes.Core
{
    public record ArticleTemplateTakenEvent(
        Guid ArticleTemplateID,
        string RoomID,
        string OriginalLanguageID,
        string TranslationLanguageID,
        List<string> Title,
        List<string> Text,
        string Source,
        string PhotoURL
    );
}