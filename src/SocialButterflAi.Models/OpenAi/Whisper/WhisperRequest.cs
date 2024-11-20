using Newtonsoft.Json;
using System.Text.Json;

namespace ButterflAi.Models.OpenAi.Whisper
{
    public class WhisperRequest
    {
        [JsonProperty("AudioFormat")]
        [JsonPropertyName("AudioFormat")]
        public AudioFormat AudioFormat { get; set; }

        [JsonProperty("Model")]
        [JsonPropertyName("Model")]
        public Model Model { get; set; }

        [JsonProperty("WavUrl")]
        [JsonPropertyName("WavUrl")]
        public string WavUrl { get; set; }

        private IEnumerable<byte> _wavData
        {
            get
            {
                var sample = WavUrl != null ? WavUrl.Substring(WavUrl.IndexOf(',') + 1) : null;

                if(sample == null)
                {
                    return null;
                }

                return Convert.FromBase64String(sample);
            }
        }

        [JsonProperty("CancellationToken")]
        [JsonPropertyName("CancellationToken")]
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
    }
}
