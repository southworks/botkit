// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.BotKit.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace Microsoft.BotKit
{
    /// <summary>
    /// A base class for a `bot` instance, an object that contains the information and functionality for taking action in response to an incoming message.
    /// Note that adapters are likely to extend this class with additional platform-specific methods - refer to the adapter documentation for these extensions.
    /// </summary>
    public class BotWorker
    {
        private BotWorkerConfiguration config;

        public Botkit Controller { get; }

        /// <summary>
        /// Create a new BotWorker instance. Do not call this directly - instead, use controller.spawn().
        /// </summary>
        /// <param name="controller">A pointer to the main Botkit controller</param>
        /// <param name="config">An object typically containing { dialogContext, reference, context, activity }</param>
        public BotWorker(Botkit controller, BotWorkerConfiguration config)
        {
            Controller = controller;
            this.config = config;
        }

        public Activity GetActivity()
        {
            return config.Activity;
        }

        /// Send a message using whatever context the bot was spawned in or set using changeContext() --
        /// or more likely, one of the platform-specific helpers like startPrivateConversation() (Slack), startConversationWithUser() (Twilio SMS), and 
        /// startConversationWithUser() (Facebook Messenger) Be sure to check the platform documentation for others - most adapters include at least one.
        /// </summary>
        /// <param name="message">A BotkitSlackMessage containing the text of a reply, or more fully formed message object</param>
        /// <returns>Return value will contain the results of the send action, typically {id: <id of message>}</returns>
        public async Task<ResourceResponse> Say(BotkitSlackMessage message)
        {
            var activity = EnsureMessageFormat(message);

            return await config.TurnContext.SendActivityAsync(activity);
        }

        public async Task<ResourceResponse> Say(Activity message)
        {
            return await config.TurnContext.SendActivityAsync(message);
        }

        /// <summary>
        /// Send a message using whatever context the bot was spawned in or set using changeContext() --
        /// or more likely, one of the platform-specific helpers like startPrivateConversation() (Slack), startConversationWithUser() (Twilio SMS), and 
        /// startConversationWithUser() (Facebook Messenger) Be sure to check the platform documentation for others - most adapters include at least one.
        /// </summary>
        /// <param name="message">A string containing the text of a reply, or more fully formed message object</param>
        /// <returns>Return value will contain the results of the send action, typically {id: <id of message>}</returns>
        public async Task<object> Say(string message)
        {
            var activity = EnsureMessageFormat(message);

            return await config.TurnContext.SendActivityAsync(activity);
        }

        /// <summary>
        /// Reply to an incoming message.
        /// Message will be sent using the context of the source message, which may in some cases be different than the context used to spawn the bot.
        /// </summary>
        /// <param name="message">An incoming message, usually passed in to a handler function</param>
        /// <param name="response">A string containing the text of a reply, or more fully formed message object</param>
        /// <returns>Return value will contain the results of the send action, typically {id: <id of message>}</returns>
        public async Task<object> ReplyAsync(BotkitSlackMessage message, BotkitSlackMessage response)
        {
            var activity = EnsureMessageFormat(response);
            var reference = message.IncomingMessage.GetConversationReference();
            activity = activity.ApplyConversationReference(reference);

            return await Say(activity);
        }

        /// <summary>
        /// Begin a pre-defined dialog by specifying its id. The dialog will be started in the same context (same user, same channel) in which the original incoming message was received.
        /// See "Using Dialogs" in the core documentation.
        /// </summary>
        /// <param name="id">ID of dialog</param>
        /// <param name="options">Object containing options to be passed into the dialog</param>
        public async Task BeginDialogAsync(string id, Dictionary<string, object> options)
        {
            if (config.DialogContext == null)
            {
                throw new Exception("Call to beginDialog on a bot that did not receive a dialogContext during spawn");
            }

            var opt = new Dictionary<string, object>();

            // TO-DO: Review this implementation of 'Options' as Dictionary
            foreach (KeyValuePair<string, object> entry in options)
            {
                opt.Add(entry.Key, entry.Value);
            }

            opt.Add("user", config.TurnContext.Activity.From.Id);
            opt.Add("channel", config.TurnContext.Activity.Conversation.Id);

            await config.DialogContext.BeginDialogAsync(id + ":botkit-wrapper", options);

            // make sure we save the state change caused by the dialog.
            // this may also get saved again at end of turn
            await Controller.SaveState(this);
        }

        /// <summary>
        /// Begin a pre-defined dialog by specifying its id. The dialog will be started in the same context (same user, same channel) in which the original incoming message was received.
        /// See "Using Dialogs" in the core documentation.
        /// </summary>
        /// <param name="id">ID of dialog</param>
        public async Task BeginDialogAsync(string id)
        {
            if (config.DialogContext == null)
            {
                throw new Exception("Call to beginDialog on a bot that did not receive a dialogContext during spawn");
            }

            var opt = new Dictionary<string, object>();
            opt.Add("user", config.TurnContext.Activity.From.Id);
            opt.Add("channel", config.TurnContext.Activity.Conversation.Id);

            await config.DialogContext.BeginDialogAsync(id + ":botkit-wrapper");

            // make sure we save the state change caused by the dialog.
            // this may also get saved again at end of turn
            await Controller.SaveState(this);
        }

        /// <summary>
        /// Cancel any and all active dialogs for the current user/context.
        /// </summary>
        public async Task<DialogTurnResult> CancelAllDialogsAsync()
        {
            return (config.DialogContext != null)
                ? await config.DialogContext.CancelAllDialogsAsync()
                : throw new Exception("Call to CancelAllDialogs on a bot that did not receive a dialogContext during spawn");
        }

        /// <summary>
        /// Replace any active dialogs with a new a pre-defined dialog by specifying its id. The dialog will be started in the same context (same user, same channel) in which the original incoming message was received.
        /// See "Using Dialogs" in the core documentation.
        /// </summary>
        /// <param name="id">ID of dialog</param>
        /// <param name="options">Object containing options to be passed into the dialog</param>
        public async Task ReplaceDialogAsync(string id, Dictionary<string, object> options)
        {
            if (config.DialogContext == null)
            {
                throw new Exception("Call to beginDialog on a bot that did not receive a dialogContext during spawn");
            }

            var opt = new Dictionary<string, object>();

            // TO-DO: Review this implementation of 'Options' as Dictionary
            foreach (KeyValuePair<string, object> entry in options)
            {
                opt.Add(entry.Key, entry.Value);
            }

            opt.Add("user", config.TurnContext.Activity.From.Id);
            opt.Add("channel", config.TurnContext.Activity.Conversation.Id);

            await config.DialogContext.ReplaceDialogAsync(id + ":botkit-wrapper", opt);

            // make sure we save the state change caused by the dialog.
            // this may also get saved again at end of turn
            await Controller.SaveState(this);
        }

        /// <summary>
        /// Alter the context in which a bot instance will send messages.
        /// Use this method to create or adjust a bot instance so that it can send messages to a predefined user/channel combination.
        /// </summary>
        /// <param name="conversationReference">A ConversationReference, most likely captured from an incoming message and stored for use in proactive messaging scenarios.</param>
        public async Task<BotWorker> ChangeContextAsync(ConversationReference conversationReference)
        {
            // change context of outbound activities to use this new address
            config.ConversationReference = conversationReference;

            // Create an activity using this reference
            var activity = new Activity();
            activity = activity.ApplyConversationReference(conversationReference, true);

            // create a turn context
            var turnContext = new TurnContext(Controller.Adapter, activity);

            // create a new dialogContext so beginDialog works.
            var dialogContext = await Controller.DialogSet.CreateContextAsync(turnContext);

            config.TurnContext = turnContext;
            config.DialogContext = dialogContext;
            config.Activity = activity;

            return this;
        }

        public async Task StartConversationWithUserAsync(ConversationReference convReference)
        {
            if(string.IsNullOrEmpty(convReference.ServiceUrl))
            {
                throw new Exception("bot.startConversationWithUser(): missing serviceUrl.");
            }

            // Create conversation
            var parameters = new ConversationParameters()
            {
                Bot = convReference.Bot,
                Members = new List<ChannelAccount>() { convReference.User },
                IsGroup = false,
                Activity = null,
                ChannelData = null
            };

            ConnectorClient client = new ConnectorClient(new Uri(convReference.ServiceUrl));

            // Mix in the tenant ID if specified. This is required for MS Teams.
            if (convReference.Conversation != null && !string.IsNullOrEmpty(convReference.Conversation.TenantId))
            {
                // Putting tenantId in channelData is a temporary solution while we wait for the Teams API to be updated
                // TO-DO: Should we implement the temporary solution while when have a permanent one?
                //parameters.channelData = { tenant: { id: reference.conversation.tenantId } };

                // Permanent solution is to put tenantId in parameters.tenantId
                parameters.TenantId = convReference.Conversation.TenantId;
            }

            var response = await client.Conversations.CreateConversationAsync(parameters);

            // Initialize request and copy over new conversation ID and updated serviceUrl.
            var request = new Activity()
            {
                Type = "event",
                Name = "createConversation"
            };

            request.ApplyConversationReference(convReference, true);

            var conversation = new ConversationAccount()
            {
                Id = response.Id,
                IsGroup = false,
                ConversationType = null,
                TenantId = null,
                Name = null
            };

            request.Conversation = conversation;

            if (!string.IsNullOrEmpty(response.ServiceUrl))
            {
                request.ServiceUrl = response.ServiceUrl;
            }

            // Create context and run middleware
            using (var turnContext = new TurnContext(Controller.Adapter, request))
            {
                // create a new dialogContext so beginDialog works.
                var convoState = new ConversationState(new MemoryStorage());
                var dialogStateProperty = convoState.CreateProperty<DialogState>("dialogstate");
                var ds = new DialogSet(dialogStateProperty);
                var dialogContext = await ds.CreateContextAsync(turnContext);

                config.TurnContext = turnContext;
                config.DialogContext = dialogContext;
                config.Activity = request;
            }
        }

        /// <summary>
        /// Take a crudely-formed Botkit message with any sort of field (may just be a string, may be a partial message object)
        /// and map it into a beautiful BotFramework Activity.
        /// Any fields not found in the Activity definition will be moved to activity.channelData.
        /// </summary>
        /// <param name="message">Message a string or partial outgoing message object</param>
        /// <returns>A properly formed Activity object</returns>
        public Activity EnsureMessageFormat(BotkitSlackMessage message)
        {
            var activity = new Activity()
            {
                Type = message.Type,
                Text = message.Text,
                AttachmentLayout = message.AttachmentLayout,
                Attachments = message.Attachments,
                SuggestedActions = message.SuggestedActions,
                Speak = message.Speak,
                InputHint = message.InputHint,
                Summary = message.Summary,
                TextFormat = message.TextFormat,
                Importance = message.Importance,
                DeliveryMode = message.DeliveryMode,
                Expiration = message.Expiration,
                Value = message.Value
            };

            activity.ChannelData = AssignChannelData(activity.ChannelData, message);

            return activity;
        }

        public Activity EnsureMessageFormat(string message)
        {
            return new Activity()
            {
                Type = "message",
                Text = message,
                ChannelData = null
            };
        }

        private object AssignChannelData(object target, BotkitSlackMessage source)
        {
            // Copy all the current properties to the new ExpandoObject
            var newChannelData = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)newChannelData;

            foreach (var property in target.GetType().GetProperties())
                dictionary.Add(property.Name, property.GetValue(target));

            // Copy the properties of the source ChannelData and copy it to newChannelData if it doesnt exist
            foreach (var property in source.GetType().GetProperties())
            {
                if (newChannelData.GetType().GetProperty(property.Name) != null)
                {
                    dictionary.Add(property.Name, property.GetValue(source));
                }
            }

            return newChannelData;
        }

        /// <summary>
        /// Set the HTTP response status code for this turn
        /// </summary>
        /// <param name="status">A valid HTTP status code like 200 202 301 500 etc</param>
        public void HTTPStatus(int status)
        {
            config.TurnContext.TurnState["httpStatus"] = status;
        }

        /// <summary>
        /// Set the http response body for this turn.
        /// Use this to define the response value when the platform requires a synchronous response to the incoming webhook.
        /// </summary>
        /// <param name="any"></param>
        public void HTTPBody(object body)
        {
            config.TurnContext.TurnState["httpStatus"] = body;
        }
    }
}
