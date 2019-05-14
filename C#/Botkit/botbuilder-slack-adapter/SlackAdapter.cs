using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botbuilder_slack_adapter
{
    public class SlackAdapter : BotAdapter
    {
        /// <summary>
        /// Create a Slack adapter.
        /// </summary>
        /// <param name="options">An object containing API credentials, a webhook verification token and other options</param>
        public SlackAdapter(ISlackAdapterOptions options) : base()
        {

        }

        /// <summary>
        /// Get a Slack API client with the correct credentials based on the team identified in the incoming activity.
        /// This is used by many internal functions to get access to the Slack API, and is exposed as `bot.api` on any bot worker instances.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<object> GetAPI(Activity activity)
        {
            return new object();
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

        }

        /// <summary>
        /// Get the oauth link for this bot, based on the clientId and scopes passed in to the constructor.
        /// </summary>
        /// <returns>A url pointing to the first step in Slack's oauth flow.</returns>
        public string GetInstallLink()
        {
            return "";
        }

        /// <summary>
        /// Validates an oauth code sent by Slack during the install process.
        /// An example using Botkit's internal webserver to configure the /install/auth route:
        /// </summary>
        /// <param name="code">The value found in `req.query.code` as part of Slack's response to the oauth flow.</param>
        public async Task<object> ValidateOauthCode(string code)
        {
            return new object();
        }

        /// <summary>
        /// Formats a BotBuilder activity into an outgoing Slack message.
        /// </summary>
        /// <param name="activity">A BotBuilder Activity object</param>
        /// <returns>A Slack message object with {text, attachments, channel, thread_ts} as well as any fields found in activity.channelData</returns>
        public object ActivityToSlack(Activity activity)
        {
            return new object();
        }

        /// <summary>
        /// Standard BotBuilder adapter method to send a message from the bot to the messaging API.
        /// </summary>
        /// <param name="context">A TurnContext representing the current incoming message and environment.</param>
        /// <param name="activities">An array of outgoing activities to be sent back to the messaging API.</param>
        public async Task<ResourceResponse[]> SendActivities(TurnContext context, Activity[] activities)
        {

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
    }
}
