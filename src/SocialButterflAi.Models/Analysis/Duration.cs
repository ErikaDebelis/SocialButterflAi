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
        public TimeSpan TimeSpan => TimeSpan.Parse(EndTime) - TimeSpan.Parse(StartTime);

        public ProcessStartInfo ProcessStartInfo { get; set; }
    }
}