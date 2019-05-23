// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using SlackAPI;
using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using BotkitLibrary;

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
        public async Task<SlackAPI> GetAPIAsync(Activity activity)
        {
            if (Slack != null)
            {
                return Slack;
            }
            else if ((activity.Conversation as dynamic).team != null)
            {
                var token = await options.GetTokenForTeam((activity.Conversation as dynamic).team);
                return string.IsNullOrEmpty(token)? new SlackAPI(token) : throw new Exception("Missing credentials for team.");
            }
            else
            {
                Console.WriteLine("Unable to create API based on activity: ", activity);
                return null;
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
            else if ((activity.Conversation as dynamic).team != null)
            {
                var userID = await options.GetBotUserByTeam((activity.Conversation as dynamic).team);
                return string.IsNullOrEmpty(userID) ? userID : throw new Exception("Missing credentials for team.");
            }
            else
            {
                Console.WriteLine("Could not find bot user id based on activity: ", activity);
                return null;
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
            SlackAPI slack = new SlackAPI();
            var results = await slack.oauth.access(); // TODO: Implement 'slack.oauth.access' in 'SlackApi'
            if (results.ok)
            {
                return results;
            }
            else
            {
                throw new Exception(results.error);
            }
        }

        /// <summary>
        /// Formats a BotBuilder activity into an outgoing Slack message.
        /// </summary>
        /// <param name="activity">A BotBuilder Activity object</param>
        /// <returns>A Slack message object with {text, attachments, channel, thread_ts} as well as any fields found in activity.channelData</returns>
        public object ActivityToSlack(Activity activity)
        {
            var channelId = activity.Conversation.Id;
            var threadTS = (activity.Conversation as dynamic).threadTS;

            dynamic message = new ExpandoObject();
            message.TS = activity.Id;
            message.Text = activity.Text;
            message.Attachments = activity.Attachments;
            message.Channel = channelId;
            message.ThreadTS = threadTS;

            // if channelData is specified, overwrite any fields in message object
            if (activity.ChannelData != null)
            {
                message = activity.ChannelData;
            }

            // should this message be sent as an ephemeral message
            if (message.ephemeral)
            {
                message.User = activity.Recipient.Id;
            }

            if (message.icon_url || message.icon_emoji || message.username)
            {
                message.as_user = false;
            }

            return message;
        }

        /// <summary>
        /// Standard BotBuilder adapter method to send a message from the bot to the messaging API.
        /// </summary>
        /// <param name="context">A TurnContext representing the current incoming message and environment.</param>
        /// <param name="activities">An array of outgoing activities to be sent back to the messaging API.</param>
        public override async Task<ResourceResponse[]> SendActivitiesAsync(ITurnContext turnContext, Activity[] activities, CancellationToken cancellationToken)
        {
            ResourceResponse[] responses = { };
            for (var i = 0; i < activities.Length; i++)
            {
                Activity activity = activities[i];
                if (activity.Type == ActivityTypes.Message)
                {
                    dynamic message = ActivityToSlack(activity as Activity);

                    try
                    {
                        SlackAPI slack = await this.GetAPIAsync(turnContext.Activity);
                        ChatPostMessageResult result = null;

                        if (message.ephemeral)
                        {
                            result = await slack.Chat
                        }
                        else
                        {

                        }

                        if (result.Ok)
                    }
                }
            }

            return responses;
        }

        /// <summary>
        /// Standard BotBuilder adapter method to update a previous message with new content.
        /// </summary>
        /// <param name="context">A TurnContext representing the current incoming message and environment.</param>
        /// <param name="activity">The updated activity in the form `{id: <id of activity to update>, ...}`</param>
        public override async Task<ResourceResponse> UpdateActivityAsync(ITurnContext turnContext, Activity activity, CancellationToken cancellationToken)
        {
            ResourceResponse results = null;
            if (activity.Id != null && activity.Conversation != null)
            {
                try
                {
                    dynamic message = ActivityToSlack(activity);
                    SlackAPI slack = await GetAPIAsync(activity);
                    results = await slack.chat.update(message);
                    if (!results.ok)
                    {
                        Console.WriteLine("Error updating activity on Slack:", results);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating activity on Slack:", ex.Message);
                }
            }
            else
            {
                throw new Exception("Cannot update activity: activity is missing id.");
            }

            return results;
        }

        /// <summary>
        /// Standard BotBuilder adapter method to delete a previous message.
        /// </summary>
        /// <param name="context">A TurnContext representing the current incoming message and environment.</param>
        /// <param name="reference">An object in the form `{activityId: <id of message to delete>, conversation: { id: <id of slack channel>}}`</param>
        public override async Task DeleteActivityAsync(ITurnContext turnContext, ConversationReference reference, CancellationToken cancellationToken)
        {
            if (reference.ActivityId != null && reference.Conversation != null)
            {
                try
                {
                    SlackAPI slack = await GetAPIAsync(turnContext.Activity);
                    // results = await slack.chat.delete({ ts: reference.activityId, channel: reference.conversation.id });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error deleting activity", ex.Message);
                    throw ex;
                }
            }
            else
            {
                throw new Exception("Cannot delete activity: reference is missing activityId.");
            }
        }

        /// <summary>
        /// Standard BotBuilder adapter method for continuing an existing conversation based on a conversation reference.
        /// </summary>
        /// <param name="reference">A conversation reference to be applied to future messages.</param>
        /// <param name="logic">A bot logic function that will perform continuing action in the form `async(context) => { ... }`</param>
        public async Task<Task> ContinueConversation(ConversationReference reference, BotCallbackHandler logic)
        {
            var request = reference.GetContinuationActivity().ApplyConversationReference(reference, true); // TODO: check on this
            
            TurnContext context = new TurnContext(this, request);

            return RunPipelineAsync(context, logic, new CancellationToken());
        }

        /// <summary>
        /// Accept an incoming webhook request and convert it into a TurnContext which can be processed by the bot's logic.
        /// </summary>
        /// <param name="req">A request object from Restify or Express</param>
        /// <param name="res">A response object from Restify or Express</param>
        /// <param name="logic">A bot logic function in the form `async(context) => { ... }`</param>
        public async void ProcessActivity(dynamic req, dynamic res, BotCallbackHandler logic)
        {
            // Create an Activity based on the incoming message from Slack.
            // There are a few different types of event that Slack might send.

            dynamic slackEvent = req.body;

            if (slackEvent.type == "url_verification")
            {
                res.status(200);
                res.send(slackEvent.challenge);
                return;
            }

            if (!VerifySignatureAsync(req, res))
            {
            }
            else if (slackEvent.payload != null)
            { 
                // handle interactive_message callbacks and block_actions
                slackEvent = JsonConvert.ToString(slackEvent.payload);
                if (options.VerificationToken != null && slackEvent.token != options.VerificationToken)
                {
                    Console.WriteLine("Rejected due to mismatched verificationToken: ", slackEvent);
                    res.status(403);
                    res.end();
                }
                else
                {
                    Activity activity = new Activity()
                    {
                        Timestamp = new DateTime(),
                        ChannelId = "slack",
                        Conversation = new ConversationAccount()
                        {
                            Id = slackEvent.channel.id
                            // thread_ts = slackEvent.thread_ts,
                            // team = slackEvent.team.id
                        },
                        From = new ChannelAccount()
                        {
                            Id = slackEvent.bot_id ? slackEvent.bot_id : slackEvent.user.id
                        },
                        Recipient = new ChannelAccount()
                        {
                            Id = null
                        },
                        ChannelData = slackEvent,
                        Type = ActivityTypes.Event
                    };

                    // this complains because of extra fields in conversation
                    activity.Recipient.Id = await GetBotUserByTeam(activity);

                    // create a conversation reference
                    var context = new TurnContext(this, activity);
                    context.TurnState.Add("httpStatus", "200");

                    await RunPipelineAsync(context, logic, new CancellationToken());

                    // send http response back
                    res.status(context.TurnState.Get<string>("httpStatus"));
                    if (context.TurnState.Get<object>("httpBody") != null)
                    {
                        res.send(context.TurnState.Get<object>("httpBody"));
                    }
                    else
                    {
                        res.end();
                    }

                }
            }
            else if (slackEvent.type == "event_callback")
            {
                // this is an event api post
                if (options.VerificationToken != null && slackEvent.token != options.VerificationToken)
                {
                    Console.WriteLine("Rejected due to mismatched verificationToken: ", slackEvent);
                    res.status(403);
                    res.end();
                }
                else
                {
                    Activity activity = new Activity()
                    {
                        Id = slackEvent.event1.ts,
                        Timestamp = new DateTime(),
                        ChannelId = "slack",
                        Conversation = new ConversationAccount()
                        {
                            Id = slackEvent.channel.id
                            // thread_ts = slackEvent.thread_ts
                        },
                        From = new ChannelAccount()
                        {
                            Id = slackEvent.event1.bot_id ? slackEvent.event1.bot_id : slackEvent.event1.user
                        },
                        Recipient = new ChannelAccount()
                        {
                            Id = null
                        },
                        ChannelData = slackEvent.event1,
                        Text = null,
                        Type = ActivityTypes.Event
                    };

                    // this complains because of extra fields in conversation
                    activity.Recipient.Id = await GetBotUserByTeam(activity);

                    // Normalize the location of the team id
                    (activity.ChannelData as dynamic).team = slackEvent.team_id;

                    // add the team id to the conversation record
                    (activity.Conversation as dynamic).team = (activity.ChannelData as dynamic).team;

                    // If this is conclusively a message originating from a user, we'll mark it as such
                    if (slackEvent.event1.type == "message" && slackEvent.event1.subtype != null)
                    {
                        activity.Type = ActivityTypes.Message;
                        activity.Text = slackEvent.event1.text;
                    }

                    // create a conversation reference
                    TurnContext context = new TurnContext(this, activity);

                    context.TurnState.Add("httpStatus", "200");

                    await RunPipelineAsync(context, logic, new CancellationToken());

                    // send http response back
                    res.status(context.TurnState.Get<string>("httpStatus"));
                    if (context.TurnState.Get<object>("httpBody") != null)
                    {
                        res.send(context.TurnState.Get<object>("httpBody"));
                    }
                    else
                    {
                        res.end();
                    }
                }
            }
            else if (slackEvent.command != null)
            {
                if (options.VerificationToken != null && slackEvent.token != options.VerificationToken)
                {
                    Console.WriteLine("Rejected due to mismatched verificationToken: ", slackEvent);
                    res.status(403);
                    res.end();
                }
                else
                {
                    // this is a slash command
                    Activity activity = new Activity()
                    {
                        Id = slackEvent.trigger_id,
                        Timestamp = new DateTime(),
                        ChannelId = "slack",
                        Conversation = new ConversationAccount()
                        {
                            Id = slackEvent.channel_id
                        },
                        From = new ChannelAccount()
                        {
                            Id = slackEvent.user_id
                        },
                        Recipient = new ChannelAccount()
                        {
                            Id = null
                        },
                        ChannelData = slackEvent,
                        Text = slackEvent.text,
                        Type = ActivityTypes.Event
                    };

                    activity.Recipient.Id = await GetBotUserByTeam(activity);

                    // Normalize the location of the team id
                    (activity.ChannelData as dynamic).team = slackEvent.team_id;

                    // add the team id to the conversation record
                    (activity.Conversation as dynamic).team = (activity.ChannelData as dynamic).team;

                    (activity.ChannelData as dynamic).BotkitEventType = "slash_command";

                    // create a conversation reference
                    TurnContext context = new TurnContext(this, activity);

                    context.TurnState.Add("httpStatus", "200");

                    await RunPipelineAsync(context, logic, new CancellationToken());

                    // send http response back
                    res.status(context.TurnState.Get<string>("httpStatus"));
                    if (context.TurnState.Get<object>("httpBody") != null)
                    {
                        res.send(context.TurnState.Get<object>("httpBody"));
                    }
                    else
                    {
                        res.end();
                    }
                }
            }
            else
            {
                Console.WriteLine("Unknown Slack event type: ", slackEvent);
            }
        }
        
        private bool VerifySignatureAsync(HttpWebRequest req, HttpWebResponse res)
        {
            /*if (options.ClientSigningSecret != null && req.rawBody)
            {
                var timestamp = req.Headers;
                var body = req.rawBody;

                object[] signature = { "v0", timestamp, body };

                string baseString = String.Join(":", signature);

                var hash = "v0=" + crypto.createHmac('sha256', this.options.clientSigningSecret);

                var retrievedSignature = req.header('X-Slack-Signature');

                // Compare the hash of the computed signature with the retrieved signature with a secure hmac compare function
                var validSignature = (): boolean => {
                    var slackSigBuffer = Buffer.from(retrievedSignature);
                    var compSigBuffer = Buffer.from(hash);

                    return crypto.timingSafeEqual(slackSigBuffer, compSigBuffer);
                };

                // replace direct compare with the hmac result
                if (!validSignature())
                {
                    Console.WriteLine("Signature verification failed, Ignoring message");
                    res.status(401);
                    return false;
                }
            }*/

            return true;
        }
    }
}
