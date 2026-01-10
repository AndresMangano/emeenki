using System;
using System.Collections.Generic;

namespace Hermes.Core
{
    public class ArticleTemplateVideoUploadedEvent
    {
        public string LanguageID { get; set; }
        public List<VideoSentence> Title { get; set; }
        public List<VideoSentence> Text { get; set; }
        public string Source { get; set; }
        public string PhotoURL { get; set; }
        public string TopicID { get; set; }
        public string VideoURL { get; set; }
    }

    public class VideoSentence
    {
        public string Text { get; set; }
        public long? StartTimeMs { get; set; }
        public long? EndTimeMs { get; set; }
    }
}




