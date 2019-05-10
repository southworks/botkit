using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace BotkitLibrary.Core
{
    public interface IBotkitMessage
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string User { get; set; }
        public string Channel { get; set; }
        public ConversationReference Reference { get; set; }
        public Activity IncomingMessage { get; set; }
    }
}
