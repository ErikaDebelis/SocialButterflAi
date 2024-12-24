using Newtonsoft.Json;
using SocialButterflAi.Models.Analysis;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.CueCoach
{
    public class MessageData
    {
        public Message Message { get; set; }

        public AnalysisData? AnalysisData { get; set; }
    }
}