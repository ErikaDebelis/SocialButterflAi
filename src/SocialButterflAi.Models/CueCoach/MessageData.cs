using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MessageDto = SocialButterflAi.Models.CueCoach.Dtos.Message;
using SocialButterflAi.Models.Analysis;

namespace SocialButterflAi.Models.CueCoach
{
    public class MessageData
    {
        public MessageDto Message { get; set; }

        public AnalysisData? AnalysisData { get; set; }
    }
}