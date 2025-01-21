using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System;

namespace SocialButterflAi.Models.Analysis
{
    public class EnhancedCaption
    {
        public Guid? Id { get; set; }
        public Guid? VideoId { get; set; }
        public Guid? AudioId { get; set; }
        public string? StandardText { get; set; }
        public string? BackgroundContext { get; set; }
        public string? SoundEffects { get; set; }
    }
}