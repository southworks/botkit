using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.BotKit
{
    public class BotWorkerConfiguration
    {
        public DialogContext DialogContext { get; set; }
        public TurnContext TurnContext { get; set; }
        public ConversationReference ConversationReference { get; set; }
        public Activity Activity { get; set; }
    }
}
