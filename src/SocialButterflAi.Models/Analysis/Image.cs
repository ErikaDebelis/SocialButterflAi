using System;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.Claude;
using System.IO;

namespace SocialButterflAi.Models.Analysis
{
    public class Image : BaseDto
    {
        public override Guid? Id { get; set; }
        public Guid UploaderIdentityId { get; set; }
        public Guid? MessageId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Base64 { get; set; }
    }
}