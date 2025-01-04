using System;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.Claude;
using System.IO;

namespace SocialButterflAi.Models.Analysis
{
    public class Audio
    {
        public Guid Id { get; set; }
        public Guid UploaderIdentityId { get; set; }
        public Guid? MessageId { get; set; }

        public string Base64 { get; set; }
    }
}