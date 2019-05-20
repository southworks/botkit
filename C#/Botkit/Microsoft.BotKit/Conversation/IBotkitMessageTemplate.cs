// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.BotKit.Conversation
{
    /// <summary>
    /// Template for defining a BotkitConversation template
    /// </summary>
    public interface IBotkitMessageTemplate
    {
        string[] Text {get; set; }
        string Action { get; set; }
        Execute Execute { get; set; }
        object[] QuickReplies { get; set; } // TO-DO: Validate this line
        object[] Attachments { get; set; }
        object ChannelData { get; set; }
        Collect Collect { get; set; }
    }

    public class Execute
    {
        public string Script { get; set; }
        public string Thread { get; set; }
    }

    public class Collect
    {
        public string Key { get; set; }
        public IBotkitConvoTrigger Options { get; set; }
    }
}
