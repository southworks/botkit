using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Rest.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BotkitLibrary
{
    public class BotkitFrameworkAdapter : BotFrameworkAdapter
    {
        /// <summary>
        /// This class extends the BotFrameworkAdapter with a few additional features to support Microsoft Teams.
        /// </summary>
        /// <param name="options"></param>
        public BotkitFrameworkAdapter(ICredentialProvider options) : base(options)
        {

        }

        /// <summary>
        /// Get the list of channels in a MS Teams team.
        /// Can only be called with a TurnContext that originated in a team conversation - 1:1 conversations happen _outside a team_ and thus do not contain the required information to call this API.
        /// </summary>
        /// <param name="turnContext">A TurnContext object representing a message or event from a user in Teams</param>
        /// <returns>An array of channels in the format [{name: string, id: string}]</returns>
        public async Task<Tuple<string, string>> GetChannels(TurnContext turnContext)
        {
            return new Tuple<string, string>("","");
        }
    }
}
