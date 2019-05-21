// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using SlackAPI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace botbuilder_slack_adapter
{
    public class SlackAdapter : BotAdapter
    {
        private readonly ISlackAdapterOptions options;
        private readonly SlackAPI Slack;
        private readonly string Identity;
        private readonly string SlackOAuthURL = "https://slack.com/oauth/authorize?client_id=";
        public Task<Action<SlackBotWorker, Task<object>>>[] Middlewares;

        /// <summary>
        /// Create a Slack adapter.
        /// </summary>
        /// <param name="options">An object containing API credentials, a webhook verification token and other options</param>
        public SlackAdapter(ISlackAdapterOptions options) : base()
        {
            this.options = options;

            if (this.options.VerificationToken != null && this.options.ClientSigningSecret != null)
            {
                string warning =
                    "****************************************************************************************" +
                    "* WARNING: Your bot is operating without recommended security mechanisms in place.     *" +
                    "* Initialize your adapter with a clientSigningSecret parameter to enable               *" +
                    "* verification that all incoming webhooks originate with Slack:                        *" +
                    "*                                                                                      *" +
                    "* var adapter = new SlackAdapter({clientSigningSecret: <my secret from slack>});       *" +
                    "*                                                                                      *" +
                    "****************************************************************************************" +
                    ">> Slack docs: https://api.slack.com/docs/verifying-requests-from-slack";

                throw new Exception(warning + Environment.NewLine + "Required: include a verificationToken or clientSigningSecret to verify incoming Events API webhooks");
            }

            if (this.options.BotToken != null)
            {
                Slack = new SlackAPI(this.options.BotToken);
                Identity = Slack.GetIdentity();
            }
            else if (
                string.IsNullOrEmpty(options.ClientId) ||
                string.IsNullOrEmpty(options.ClientSecret) ||
                string.IsNullOrEmpty(options.RedirectUri) ||
                options.Scopes.Length > 0)
            {
                throw new Exception("Missing Slack API credentials! Provide clientId, clientSecret, scopes and redirectUri as part of the SlackAdapter options.");
            }

            // TODO: migrate middleware
            //this.middlewares = {
            //    spawn: [
            //        async (bot, next) => {
            //            // make the Slack API available to all bot instances.
            //            bot.api = await this.getAPI(bot.getConfig('activity')).catch((err) => {
            //                debug('An error occurred while trying to get API creds for team', err);
            //                return next(new Error('Could not spawn a Slack API instance'));
            //            });

            //            next();
            //        }
            //    ]
            //};
        }

        /// <summary>
        /// Get a Slack API client with the correct credentials based on the team identified in the incoming activity.
        /// This is used by many internal functions to get access to the Slack API, and is exposed as `bot.api` on any bot worker instances.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<SlackAPI> GetAPI(Activity activity)
        {
            if (Slack != null)
            {
                return Slack;
            }
            else
            {
                // 'team' in 'activity.Conversation.team' is missing
                var token = ""; //await options.GetTokenForTeam(activity.Conversation.team);
                return string.IsNullOrEmpty(token) ? new SlackAPI(token) : throw new Exception("Missing credentials for team.");
            }
        }

        /// <summary>
        /// Get the bot user id associated with the team on which an incoming activity originated. This is used internally by the SlackMessageTypeMiddleware to identify direct_mention and mention events.
        /// In single-team mode, this will pull the information from the Slack API at launch.
        /// In multi-team mode, this will use the `getBotUserByTeam` method passed to the constructor to pull the information from a developer-defined source.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<string> GetBotUserByTeam(Activity activity)
        {
            if (!string.IsNullOrEmpty(Identity))
            {
                return Identity;
            }
            else
            {
                // 'team' in 'activity.Conversation.team' is missing
                var userId = ""; //await options.GetBotUserByTeam(activity.Conversation.team);
                return string.IsNullOrEmpty(userId) ? userId : throw new Exception("Missing credentials for team.");
            }
        }

        /// <summary>
        /// Get the oauth link for this bot, based on the clientId and scopes passed in to the constructor.
        /// </summary>
        /// <returns>A url pointing to the first step in Slack's oauth flow.</returns>
        public string GetInstallLink()
        {
            return (!string.IsNullOrEmpty(options.ClientId) && options.Scopes.Length > 0)
                ? SlackOAuthURL + options.ClientId + "&scope=" + string.Join(",", options.Scopes)
                : throw new Exception("getInstallLink() cannot be called without clientId and scopes in adapter options.");
        }

        /// <summary>
        /// Validates an oauth code sent by Slack during the install process.
        /// An example using Botkit's internal webserver to configure the /install/auth route:
        /// </summary>
        /// <param name="code">The value found in `req.query.code` as part of Slack's response to the oauth flow.</param>
        public async Task<object> ValidateOauthCode(string code)
        {
            // TODO: Implement 'slack.oauth.access' in 'SlackApi'
            return new object();
        }

        /// <summary>
        /// Formats a BotBuilder activity into an outgoing Slack message.
        /// </summary>
        /// <param name="activity">A BotBuilder Activity object</param>
        /// <returns>A Slack message object with {text, attachments, channel, thread_ts} as well as any fields found in activity.channelData</returns>
        public object ActivityToSlack(Activity activity)
        {
            var channelId = activity.Conversation.Id;
            // TODO: Implement Thread_TS
            var threadTs = ""; //activity.Conversation.ThreadTs

            // if channelData is specified, overwrite any fields in message object
            if (activity.ChannelData != null)
            {
                //Object.keys(activity.channelData).forEach(function(key) {
                //    message[key] = activity.channelData[key];
                //});
            }

            // should this message be sent as an ephemeral message
            //if (message.ephemeral)
            //{
            //    message.user = activity.recipient.id;
            //}

            //if (message.icon_url || message.icon_emoji || message.username)
            //{
            //    message.as_user = false;
            //}

            return new object();
        }

        /// <summary>
        /// Standard BotBuilder adapter method to send a message from the bot to the messaging API.
        /// </summary>
        /// <param name="context">A TurnContext representing the current incoming message and environment.</param>
        /// <param name="activities">An array of outgoing activities to be sent back to the messaging API.</param>
        public async Task<ResourceResponse[]> SendActivities(TurnContext context, Activity[] activities)
        {
            return new ResourceResponse[0];
        }

        /// <summary>
        /// Standard BotBuilder adapter method to update a previous message with new content.
        /// </summary>
        /// <param name="context">A TurnContext representing the current incoming message and environment.</param>
        /// <param name="activity">The updated activity in the form `{id: <id of activity to update>, ...}`</param>
        public async void UpdateActivity(TurnContext context, Activity activity)
        {

        }

        /// <summary>
        /// Standard BotBuilder adapter method to delete a previous message.
        /// </summary>
        /// <param name="context">A TurnContext representing the current incoming message and environment.</param>
        /// <param name="reference">An object in the form `{activityId: <id of message to delete>, conversation: { id: <id of slack channel>}}`</param>
        public async void DeleteActivity(TurnContext context, ConversationReference reference)
        {

        }

        /// <summary>
        /// Standard BotBuilder adapter method for continuing an existing conversation based on a conversation reference.
        /// </summary>
        /// <param name="reference">A conversation reference to be applied to future messages.</param>
        /// <param name="logic">A bot logic function that will perform continuing action in the form `async(context) => { ... }`</param>
        public async void ContinueConversation(ConversationReference reference, Action<TurnContext> logic)
        {

        }

        /// <summary>
        /// Accept an incoming webhook request and convert it into a TurnContext which can be processed by the bot's logic.
        /// </summary>
        /// <param name="req">A request object from Restify or Express</param>
        /// <param name="res">A response object from Restify or Express</param>
        /// <param name="logic">A bot logic function in the form `async(context) => { ... }`</param>
        public async void ProcessActivity(object req, object res, Action<TurnContext> logic)
        {

        }

        public override Task<ResourceResponse[]> SendActivitiesAsync(ITurnContext turnContext, Activity[] activities, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<ResourceResponse> UpdateActivityAsync(ITurnContext turnContext, Activity activity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteActivityAsync(ITurnContext turnContext, ConversationReference reference, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
