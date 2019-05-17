/**
 * Copyright(c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace botbuilder_slack_adapter
{
    /**
     * A middleware for Botkit developers using the BotBuilder SlackAdapter class.
     * This middleware causes Botkit to emit message events by their `type` or `subtype` field rather than their default BotBuilder Activity type (limited to message or event).
     * This keeps the new Botkit behavior consistent withprevious versions, and provides helpful filtering on the many event types that Slack sends.
     * To use this, bind it to the adapter before creating the Botkit controller:
     * ```C#
     * const var adapter = new SlackAdapter(options);
     * adapter.use(new SlackEventMiddleware());
     * const var controller = new Botkit({
     *      adapter: adapter,
     *      // ...
     * });
     *
     * // can bind directly to channel_join (which starts as a message with type message and subtype channel_join)
     * controller.on('channel_join', async(bot, message) => {
     *  // send a welcome
     * });
     * ```
     */

    class SlackEventMiddleware : MiddlewareSet
    {
        public async void OnTurn(TurnContext context, Func<Task<object>> next)
        {
            if ((context.Activity.Type == ActivityTypes.Event))
            {
                // Handle message sub-types
                if ((context.Activity.ChannelData as dynamic)?.subtype != null)
                {
                    (context.Activity.ChannelData as dynamic).botkitEventType = (context.Activity.ChannelData as dynamic).subtype;
                }
                else if ((context.Activity.ChannelData as dynamic)?.type != null)
                {
                    (context.Activity.ChannelData as dynamic).botkitEventType = (context.Activity.ChannelData as dynamic).type;
                }
            }

            await next();
        }
    }
}
