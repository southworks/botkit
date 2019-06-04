using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Bot.Schema;

namespace Microsoft.BotKit.Core
{
    public class BotkitSlackMessage : IBotkitMessage
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string User { get; set; }
        public string Channel { get; set; }
        public string AttachmentLayout { get; set; }
        public string Speak { get; set; }
        public string InputHint { get; set; }
        public string Summary { get; set; }
        public string TextFormat { get; set; }
        public string Importance { get; set; }
        public string DeliveryMode { get; set; }
        public object ChannelData { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public ConversationReference Reference { get; set; }
        public Activity IncomingMessage { get; set; }
        public IList<Attachment> Attachments { get; set; }
        public SuggestedActions SuggestedActions { get; set; }
    }
}
