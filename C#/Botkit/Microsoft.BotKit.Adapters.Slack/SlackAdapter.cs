// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using SlackAPI;

namespace Microsoft.BotKit.Adapters.Slack
{
    public class SlackAdapter : BotAdapter
    {
        private readonly ISlackAdapterOptions options;
        private readonly SlackTaskClient Slack;
        private readonly string Identity;
        private readonly string SlackOAuthURL = "https://slack.com/oauth/authorize?client_id=";
        public Dictionary<string, Ware> Middlewares;
        public readonly string NAME = "Slack Adapter";
        public SlackBotWorker botkitWorker; 

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
                Slack = new SlackTaskClient(this.options.BotToken);
                Identity = Slack.MySelf?.id;
            }
            else if (
                string.IsNullOrEmpty(options.ClientId) ||
                string.IsNullOrEmpty(options.ClientSecret) ||
                string.IsNullOrEmpty(options.RedirectUri) ||
                options.Scopes.Length > 0)
            {
                throw new Exception("Missing Slack API credentials! Provide clientId, clientSecret, scopes and redirectUri as part of the SlackAdapter options.");
            }

            Ware ware = new Ware();
            ware.Name = "spawn";
            ware.Middlewares = new List<Action<BotWorker, Action>>();
            ware.Middlewares.Add
                (
                    async (Bot, Next) =>
                    {
                        try
                        {
                            // make the Slack API available to all bot instances.
                            (Bot as dynamic).api = await GetAPIAsync(Bot.GetActivity());
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"An error occurred while trying to get API creds for team {ex.Message}");
                        }

                        Next();
                    }
                );

            Middlewares = new Dictionary<string, Ware>();
            Middlewares.Add(ware.Name, ware);
        }

        /// <summary>
        /// Get a Slack API client with the correct credentials based on the team identified in the incoming activity.
        /// This is used by many internal functions to get access to the Slack API, and is exposed as `bot.api` on any bot worker instances.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<SlackTaskClient> GetAPIAsync(Activity activity)
        {
            if (Slack != null)
            {
                return Slack;
            }
            else if ((activity.Conversation as dynamic).team != null)
            {
                var token = await options.GetTokenForTeam((activity.Conversation as dynamic).team);
                return !string.IsNullOrEmpty(token)? new SlackTaskClient(token) : throw new Exception("Missing credentials for team.");
            }
            
            throw new Exception($"Unable to create API based on activity:{activity}");
        }

        /// <summary>
        /// Get the bot user id associated with the team on which an incoming activity originated. This is used internally by the SlackMessageTypeMiddleware to identify direct_mention and mention events.
        /// In single-team mode, this will pull the information from the Slack API at launch.
        /// In multi-team mode, this will use the `getBotUserByTeam` method passed to the constructor to pull the information from a developer-defined source.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<string> GetBotUserByTeamAsync(Activity activity)
        {
            if (!string.IsNullOrEmpty(Identity))
            {
                return Identity;
            }
            else if ((activity.Conversation as dynamic).team != null)
            {
                var userID = await options.GetBotUserByTeam((activity.Conversation as dynamic).team);
                return !string.IsNullOrEmpty(userID) ? userID : throw new Exception("Missing credentials for team.");
            }

            throw new Exception($"Could not find bot user id based on activity:{activity}");
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
        public async Task<object> ValidateOauthCodeAsync(string code)
        {
            /*var*/ dynamic results = string.Empty; //await slack.oauth.access(); // TODO: Implement 'slack.oauth.access' in 'SlackApi'
            return results.ok ? results : throw new Exception(results.error);
        }

        /// <summary>
        /// Formats a BotBuilder activity into an outgoing Slack message.
        /// </summary>
        /// <param name="activity">A BotBuilder Activity object</param>
        /// <returns>A Slack message object with {text, attachments, channel, thread ts} as well as any fields found in activity.channelData</returns>
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
                        SlackTaskClient slack = await this.GetAPIAsync(turnContext.Activity);
                        ChatPostMessageResult result = null;

                        if (message.ephemeral)
                        {
                            // result = await slack.Chat // TODO
                        }

                        // if (result.Ok) // TODO
                    }
                    catch (Exception ex)
                    {
                        throw ex;
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
                    SlackTaskClient slack = await GetAPIAsync(activity);
                    //results = await slack.chat.update(message);
                    if (/*!results.ok*/ true)
                    {
                        Console.WriteLine("Error updating activity on Slack:", results);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
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
                    SlackTaskClient slack = await GetAPIAsync(turnContext.Activity);
                    // results = await slack.chat.delete({ ts: reference.activityId, channel: reference.conversation.id });
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting activity {ex.Message}");
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
        public async Task<Task> ContinueConversationAsync(ConversationReference reference, BotCallbackHandler logic)
        {
            var request = reference.GetContinuationActivity().ApplyConversationReference(reference, true); // TODO: check on this
            
            TurnContext context = new TurnContext(this, request);

            return RunPipelineAsync(context, logic, default(CancellationToken));
        }

        /// <summary>
        /// Accept an incoming webhook request and convert it into a TurnContext which can be processed by the bot's logic.
        /// </summary>
        /// <param name="request">A request object from Restify or Express</param>
        /// <param name="response">A response object from Restify or Express</param>
        /// <param name="logic">A bot logic function in the form `async(context) => { ... }`</param>
        public async void ProcessActivityAsync(HttpRequestMessage request, HttpResponseMessage response, BotCallbackHandler logic)
        {
            // Create an Activity based on the incoming message from Slack.
            // There are a few different types of event that Slack might send.
            dynamic slackEvent = request.Content;

            MediaTypeFormatter[] formatters = new MediaTypeFormatter[]
            {
                new JsonMediaTypeFormatter
                {
                    SupportedMediaTypes =
                    {
                        new System.Net.Http.Headers.MediaTypeHeaderValue("application/json") { CharSet = "utf-8" },
                        new System.Net.Http.Headers.MediaTypeHeaderValue("text/json") { CharSet = "utf-8" },
                    },
                },
            };

            if (slackEvent.type == "url_verification")
            {
                response.StatusCode = HttpStatusCode.OK;
                response.RequestMessage = request;
                response.Content = new ObjectContent((slackEvent.challenge as object).GetType(), slackEvent.challenge, formatters[0]);
                return;
            }

            if (!VerifySignatureAsync(request, response))
            {
            }
            else if (slackEvent.payload != null)
            { 
                // handle interactive_message callbacks and block_actions
                slackEvent = JsonConvert.ToString(slackEvent.payload);
                if (options.VerificationToken != null && slackEvent.token != options.VerificationToken)
                {
                    Console.WriteLine("Rejected due to mismatched verificationToken: ", slackEvent);
                    response.RequestMessage = request;
                    response.StatusCode = HttpStatusCode.Forbidden;
                }
                else
                {
                    Activity activity = new Activity()
                    {
                        Timestamp = new DateTime(),
                        ChannelId = "slack",
                        Conversation = new ConversationAccount()
                        {
                            Id = slackEvent.Channel.Id
                        },
                        From = new ChannelAccount()
                        {
                            Id = (slackEvent.BotId != null) ? slackEvent.BotId : slackEvent.User.Id
                        },
                        Recipient = new ChannelAccount()
                        {
                            Id = null
                        },
                        ChannelData = slackEvent,
                        Type = ActivityTypes.Event
                    };

                    // Extra fields that do not belong to activity
                    (activity.Conversation as dynamic).ThreadTs = slackEvent.ThreadTs;
                    (activity.Conversation as dynamic).Team = slackEvent.Team.Id;

                    // this complains because of extra fields in conversation
                    activity.Recipient.Id = await GetBotUserByTeamAsync(activity);

                    // create a conversation reference
                    var context = new TurnContext(this, activity);
                    context.TurnState.Add("httpStatus", "200");

                    await RunPipelineAsync(context, logic, default(CancellationToken));

                    // send http response back
                    response.RequestMessage = request;
                    response.StatusCode = (HttpStatusCode)Convert.ToInt32(context.TurnState.Get<string>("httpStatus"));
                    if (context.TurnState.Get<object>("httpBody") != null)
                    {
                        response.Content = new ObjectContent(context.TurnState.Get<string>("httpBody").GetType(), 
                                                             context.TurnState.Get<string>("httpBody"), 
                                                             formatters[0]);
                    }
                }
            }
            else if (slackEvent.Type == "event_callback")
            {
                // this is an event api post
                if (options.VerificationToken != null && slackEvent.Token != options.VerificationToken)
                {
                    Console.WriteLine("Rejected due to mismatched verificationToken: ", slackEvent);
                    response.RequestMessage = request;
                    response.StatusCode = HttpStatusCode.Forbidden;
                    response.Content = new ObjectContent(typeof(string), string.Empty, formatters[0]);
                }
                else
                {
                    Activity activity = new Activity()
                    {
                        Id = slackEvent.Event.Ts,
                        Timestamp = new DateTime(),
                        ChannelId = "slack",
                        Conversation = new ConversationAccount()
                        {
                            Id = slackEvent.Channel.Id
                        },
                        From = new ChannelAccount()
                        {
                            Id = slackEvent.Event.BotId ? slackEvent.Event.BotId : slackEvent.Event.User
                        },
                        Recipient = new ChannelAccount()
                        {
                            Id = null
                        },
                        ChannelData = slackEvent.Event,
                        Text = null,
                        Type = ActivityTypes.Event
                    };

                    // Extra field that doesn't belong to activity
                    (activity.Conversation as dynamic).ThreadTs = slackEvent.ThreadTs;

                    // this complains because of extra fields in conversation
                    activity.Recipient.Id = await GetBotUserByTeamAsync(activity);

                    // Normalize the location of the team id
                    (activity.ChannelData as dynamic).team = slackEvent.TeamId;

                    // add the team id to the conversation record
                    (activity.Conversation as dynamic).team = (activity.ChannelData as dynamic).team;

                    // If this is conclusively a message originating from a user, we'll mark it as such
                    if (slackEvent.Event.Type == "message" && slackEvent.Event.Subtype != null)
                    {
                        activity.Type = ActivityTypes.Message;
                        activity.Text = slackEvent.Event.Text;
                    }

                    // create a conversation reference
                    TurnContext context = new TurnContext(this, activity);

                    context.TurnState.Add("httpStatus", "200");

                    await RunPipelineAsync(context, logic, default(CancellationToken));

                    // send http response back
                    response.RequestMessage = request;
                    response.StatusCode = (HttpStatusCode)Convert.ToInt32(context.TurnState.Get<string>("httpStatus"));
                    if (context.TurnState.Get<object>("httpBody") != null)
                    {
                        var messageBody = context.TurnState.Get<object>("httpBody");
                        response.Content = new ObjectContent(messageBody.GetType(), messageBody, formatters[0]);
                    }
                }
            }
            else if (slackEvent.Command != null)
            {
                if (options.VerificationToken != null && slackEvent.Token != options.VerificationToken)
                {
                    Console.WriteLine("Rejected due to mismatched verificationToken: ", slackEvent);
                    response.RequestMessage = request;
                    response.StatusCode = HttpStatusCode.Forbidden;
                }
                else
                {
                    // this is a slash command
                    Activity activity = new Activity()
                    {
                        Id = slackEvent.TriggerId,
                        Timestamp = new DateTime(),
                        ChannelId = "slack",
                        Conversation = new ConversationAccount()
                        {
                            Id = slackEvent.ChannelId
                        },
                        From = new ChannelAccount()
                        {
                            Id = slackEvent.UserId
                        },
                        Recipient = new ChannelAccount()
                        {
                            Id = null
                        },
                        ChannelData = slackEvent,
                        Text = slackEvent.text,
                        Type = ActivityTypes.Event
                    };

                    activity.Recipient.Id = await GetBotUserByTeamAsync(activity);

                    // Normalize the location of the team id
                    (activity.ChannelData as dynamic).team = slackEvent.TeamId;

                    // add the team id to the conversation record
                    (activity.Conversation as dynamic).team = (activity.ChannelData as dynamic).team;

                    (activity.ChannelData as dynamic).BotkitEventType = "slash_command";

                    // create a conversation reference
                    TurnContext context = new TurnContext(this, activity);

                    context.TurnState.Add("httpStatus", "200");

                    await RunPipelineAsync(context, logic, default(CancellationToken));

                    // send http response back
                    response.RequestMessage = request;
                    response.StatusCode = (HttpStatusCode)Convert.ToInt32(context.TurnState.Get<string>("httpStatus"));
                    if (context.TurnState.Get<object>("httpBody") != null)
                    {
                        var messageBody = context.TurnState.Get<object>("httpBody");
                        response.Content = new ObjectContent(messageBody.GetType(), messageBody, formatters[0]);
                    }
                    else
                    {
                        response.Content = new ObjectContent(typeof(string), string.Empty, formatters[0]);
                    }
                }
            }
            else
            {
                throw new Exception($"Unknown Slack event type:{slackEvent}");
            }
        }
        
        private bool VerifySignatureAsync(HttpRequestMessage request, HttpResponseMessage response)
        {
            if (options.ClientSigningSecret != null && request.Content != null)
            {
                var timestamp = request.Headers;
                var body = request.Content;

                object[] signature = { "v0", timestamp, body };

                string baseString = String.Join(":", signature);

                HashAlgorithm algorithm = SHA256.Create();
                var hash = "v0=" + algorithm.ComputeHash(Encoding.UTF8.GetBytes(options.ClientSigningSecret));
                var retrievedSignature = request.Headers.GetValues("X-Slack-Signature");

                // Compare the hash of the computed signature with the retrieved signature with a secure hmac compare function
                bool signatureIsValid = String.Equals(hash, retrievedSignature);

                // replace direct compare with the hmac result
                if (!signatureIsValid)
                {
                    Console.WriteLine("Signature verification failed, Ignoring message");
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    return false;
                }
            }

            return true;
        }
    }
}
