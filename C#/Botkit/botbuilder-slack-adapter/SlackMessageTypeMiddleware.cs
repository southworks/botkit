using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botbuilder_slack_adapter
{
    public class SlackMessageTypeMiddleware : MiddlewareSet
    {
        /// <summary>
        /// Not for direct use - implements the MiddlewareSet's required onTurn function used to process the event
        /// </summary>
        /// <param name="context"></param>
        /// <param name=""></param>
        public async void OnTurn(TurnContext context, Func<object>)
        {

        }
    }
}
