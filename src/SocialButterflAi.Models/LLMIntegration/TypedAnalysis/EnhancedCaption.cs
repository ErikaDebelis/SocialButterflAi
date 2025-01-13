using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System;

namespace SocialButterflAi.Models.LLMIntegration.TypedAnalysis
{
    public class EnhancedCaption
    {
        public Guid? Id { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? StandardText { get; set; }
        public string? BackgroundContext { get; set; }
        public string? SoundEffects { get; set; }
    }
}