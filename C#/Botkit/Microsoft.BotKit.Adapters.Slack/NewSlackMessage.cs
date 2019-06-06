using SlackAPI.WebSocketMessages;

namespace Microsoft.BotKit.Adapters.Slack
{
    public class NewSlackMessage : NewMessage
    {
        public string Ephemeral { get; set; }
        public string AsUser { get; set; }
        public string IconUrl { get; set; }
        public string IconEmoji { get; set; }
    }
}
