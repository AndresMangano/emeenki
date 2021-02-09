using System;

namespace Hermes.Worker.Core.Repositories
{
    public interface IArticleTemplateSentenceRepository
    {
        void InsertArticleTemplateSentence(Guid articleTemplateID, bool inText, int sentenceIndex, string sentence);
    }
}