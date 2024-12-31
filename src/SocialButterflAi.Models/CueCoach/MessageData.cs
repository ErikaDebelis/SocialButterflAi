using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.Analysis;

namespace SocialButterflAi.Models.CueCoach
{
    public class MessageData
    {
        public Message Message { get; set; }

        public AnalysisData? AnalysisData { get; set; }
    }
}