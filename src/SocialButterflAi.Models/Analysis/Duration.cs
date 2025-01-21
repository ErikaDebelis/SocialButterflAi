using System;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace SocialButterflAi.Models.Analysis
{
    public class DurationData
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public ProcessStartInfo ProcessStartInfo { get; set; }
    }
}