using System;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.Claude;
using System.IO;

namespace SocialButterflAi.Models.Analysis
{
    public class VideoDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UploaderIdentityId { get; set; }
        public Guid? RelatedChatId { get; set; }


        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public VideoFormat Format { get; set; }
        public FileStream FileStream { get; set; }
        public string Base64Audio { get; set; }
        public DurationData Duration { get; set; }
    }
}