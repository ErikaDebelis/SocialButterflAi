using System;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.Claude;
using System.IO;

namespace SocialButterflAi.Models.Analysis
{
    public class Video: BaseDto
    {
        [JsonProperty("Id")]
        [JsonPropertyName("Id")]
        public override Guid? Id { get; set; }
        public Guid UploaderIdentityId { get; set; }
        public Guid? MessageId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public VideoFormat Format { get; set; }
        public FileStream FileStream { get; set; }
        public string Base64 { get; set; }
        public DurationData Duration { get; set; }
    }
}