// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.BotKit.Adapters.Slack
{
    /// <summary>
    /// A middleware for Botkit developers using the BotBuilder SlackAdapter class.
    /// This middleware causes Botkit to emit more specialized events for the different types of message that Slack might send.
    /// Responsible for classifying messages:
    ///      * `direct_message` events are messages received through 1:1 direct messages with the bot
    ///      * `direct_mention` events are messages that start with a mention of the bot, i.e "@mybot hello there"
    ///      * `mention` events are messages that include a mention of the bot, but not at the start, i.e "hello there @mybot"
    /// In addition, messages from bots and changing them to `bot_message` events. All other types of message encountered remain `message` events.
    /// </summary>
    public class SlackMessageTypeMiddleware : MiddlewareSet
    {
        /// <summary>
        /// Not for direct use - implements the MiddlewareSet's required onTurn function used to process the event
        /// </summary>
        /// <param name="context"></param>
        /// <param name=""></param>
        public async void OnTurn(TurnContext context, NextDelegate next, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (context.Activity.Type == "message" && context.Activity.ChannelData != null)
            {
                var adapter = context.Adapter as SlackAdapter;

                string botUserId = await adapter.GetBotUserByTeam(context.Activity);
                var mentionSyntax = "<@" + botUserId + "(\\|.*?)?>";
                var mention = new Regex(mentionSyntax, RegexOptions.IgnoreCase);
                var directMention = new Regex('^' + mentionSyntax, RegexOptions.IgnoreCase);

                // is this a DM, a mention, or just ambient messages passing through?
                if ((context.Activity.ChannelData as dynamic)?.channel_type == "im")
                {
                    (context.Activity.ChannelData as dynamic).botkitEventType = "direct_message";

                    // strip any potential leading @mention
                    Regex.Replace(
                        Regex.Replace(
                            Regex.Replace(
                                Regex.Replace(context.Activity.Text, directMention.ToString(), ""), 
                                @"/ ^\s +/", ""), 
                            @"/ ^:\s +/", ""), 
                        @"/ ^\s +/", "");
                }
                else if (!string.IsNullOrEmpty(botUserId) && !string.IsNullOrEmpty(context.Activity.Text) && context.Activity.Text.Equals(directMention))
                {
                    (context.Activity.ChannelData as dynamic).botkitEventType = "direct_mention";

                    // strip the @mention
                    Regex.Replace(
                        Regex.Replace(
                            Regex.Replace(
                                Regex.Replace(context.Activity.Text, directMention.ToString(), ""),
                                @"/ ^\s +/", ""),
                            @"/ ^:\s +/", ""),
                        @"/ ^\s +/", "");
                }
                else if (!string.IsNullOrEmpty(botUserId) && string.IsNullOrEmpty(context.Activity.Text) && context.Activity.Text.Equals(mention))
                {
                    (context.Activity.ChannelData as dynamic).botkitEventType = "mention";
                }

                // if this is a message from a bot, we probably want to ignore it.
                // switch the botkit event type to bot_message
                // and the activity type to Event <-- will stop it from being included in dialogs
                // NOTE: This catches any message from any bot, including this bot.
                // Note also, botId here is not the same as bot_user_id so we can't (yet) identify messages originating from this bot without doing an additional API call.
                if ((context.Activity.ChannelData as dynamic)?.botId != null)
                {
                    (context.Activity.ChannelData as dynamic).botkitEventType = "bot_message";
                    context.Activity.Type = ActivityTypes.Event;
                }
            }
            await next(cancellationToken).ConfigureAwait(false);
        }
    }
}
