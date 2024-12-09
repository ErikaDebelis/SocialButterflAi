using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper
{
    public enum AudioFormat
    {
        unknown,
        mp3,
        mp4,
        mpeg,
        mpga,
        wav,
        webm
    }
}