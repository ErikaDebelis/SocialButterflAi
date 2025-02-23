using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Threading;


namespace SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper
{
    public class WhisperRequest
    {
        [JsonProperty("AudioFormat")]
        [JsonPropertyName("AudioFormat")]
        public AudioFormat AudioFormat { get; set; }

        [JsonProperty("Model")]
        [JsonPropertyName("Model")]
        public Model Model { get; set; }

        // [JsonProperty("WavUrl")]
        // [JsonPropertyName("WavUrl")]
        // public string WavUrl { get; set; }

        [JsonProperty("Base64Audio")]
        [JsonPropertyName("Base64Audio")]
        public string Base64Audio { get; set; }

        private IEnumerable<byte> _wavBytes
        {
            get
            {
                // var sample = WavUrl != null ? WavUrl.Substring(WavUrl.IndexOf(',') + 1) : null;

                // if(sample == null)
                // {
                //     return null;
                // }

                return Convert.FromBase64String(Base64Audio);
            }
        }

        [JsonProperty("CancellationToken")]
        [JsonPropertyName("CancellationToken")]
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
    }
}