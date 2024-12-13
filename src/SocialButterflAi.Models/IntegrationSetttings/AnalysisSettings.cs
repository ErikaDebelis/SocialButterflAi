using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.IntegrationSettings
{
    public class VideoSettings
    {
        public string MaxFileSize { get; set; }
        public string UploadPath { get; set; }
        public string ProcessedPath { get; set; }
    }

    public class AnalysisSettings
    {
        public VideoSettings VideoSettings { get; set; }
    }
}