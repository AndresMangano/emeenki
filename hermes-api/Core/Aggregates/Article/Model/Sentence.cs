using System.Collections.Generic;

namespace Hermes.Core
{
    public class Sentence
    {
        public int Index { get; set; }
        public string OriginalText { get; set; }
        public List<Translation> TranslationHistory { get; set; }
    }
}