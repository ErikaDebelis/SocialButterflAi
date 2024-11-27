using Newtonsoft.Json;
using System.Text.Json;

namespace SocialButterflAi.Models.Claude
{
    public enum StopReason
    {
        unknown,
        end_turn,
        max_tokens,
        stop_sequence,
        tool_use
    }
}