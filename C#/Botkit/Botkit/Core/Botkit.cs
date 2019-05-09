using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BotkitLibrary.Core
{
    public class Botkit
    {
        public string Version = "Not yet implemented";

        /// <summary>
        /// Middleware endpoints available for plugins and features to extend Botkit.
        /// Endpoints available are: spawn, ingest, receive, send.
        /// </summary>
        public Middleware Middleware { get; set; }

        /// <summary>
        /// A BotBuilder storage driver - defaults to MemoryStorage
        /// </summary>
        public IStorage Storage { get; set; }

        /// <summary>
        /// An Express webserver
        /// </summary>
        public object Webserver { get; set; }

        /// <summary>
        /// A direct reference to the underlying HTTP server object
        /// </summary>
        public object HTTP { get; set; }

        /// <summary>
        /// Any BotBuilder-compatible adapter - defaults to a BotFrameworkAdapter
        /// </summary>
        public object Adapter { get; set; }

        /// <summary>
        /// A BotBuilder DialogSet that serves as the top level dialog container for the Botkit app
        /// </summary>
        public DialogSet DialogSet { get; set; }
    }
}
