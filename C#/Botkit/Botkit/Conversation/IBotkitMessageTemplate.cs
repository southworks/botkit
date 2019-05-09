using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotkitLibrary.Conversation
{
    /// <summary>
    /// Template for defining a BotkitConversation template
    /// </summary>
    public interface IBotkitMessageTemplate
    {
        string[] Text {get; set; }
        string Action { get; set; }
        Execute Execute { get; set; }
        object[] QuickReplies { get; set; } // Validate this line
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
