using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotkitLibrary.Core;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace BotkitLibrary
{
    /// <summary>
    /// A base class for a `bot` instance, an object that contains the information and functionality for taking action in response to an incoming message.
    /// Note that adapters are likely to extend this class with additional platform-specific methods - refer to the adapter documentation for these extensions.
    /// </summary>
    public class BotWorker
    {
        private Botkit Controller { get; set; }

        /// <summary>
        /// Create a new BotWorker instance. Do not call this directly - instead, use controller.spawn().
        /// </summary>
        /// <param name="controller">A pointer to the main Botkit controller</param>
        /// <param name="config">An object typically containing { dialogContext, reference, context, activity }</param>
        public BotWorker(Botkit controller, object config)
        {
            Controller = controller;
        }

        /// <summary>
        /// Get a reference to the main Botkit controller.
        /// </summary>
        /// <returns></returns>
        public Botkit GetController()
        {
            return Controller;
        }

        /// <summary>
        /// Get a value from the BotWorker's configuration.
        /// </summary>
        /// <param name="key">The name of a value stored in the configuration</param>
        /// <returns>The value stored in the configuration (or null if absent)</returns>
        public object GetConfig(string key)
        {
            return new object();
        }

        /// <summary>
        /// Send a message using whatever context the bot was spawned in or set using changeContext() --
        /// or more likely, one of the platform-specific helpers like startPrivateConversation() (Slack), startConversationWithUser() (Twilio SMS), and 
        /// startConversationWithUser() (Facebook Messenger) Be sure to check the platform documentation for others - most adapters include at least one.
        /// </summary>
        /// <param name="message">A string containing the text of a reply, or more fully formed message object</param>
        /// <returns>Return value will contain the results of the send action, typically {id: <id of message>}</returns>
        public async Task<object> Say(IBotkitMessage message)
        {
            return new object();
        }

        /// <summary>
        /// Reply to an incoming message.
        /// Message will be sent using the context of the source message, which may in some cases be different than the context used to spawn the bot.
        /// </summary>
        /// <param name="message">An incoming message, usually passed in to a handler function</param>
        /// <param name="response">A string containing the text of a reply, or more fully formed message object</param>
        /// <returns>Return value will contain the results of the send action, typically {id: <id of message>}</returns>
        public async Task<object> Reply(IBotkitMessage message, IBotkitMessage response)
        {
            return new object();
        }

        /// <summary>
        /// Begin a pre-defined dialog by specifying its id. The dialog will be started in the same context (same user, same channel) in which the original incoming message was received.
        /// See "Using Dialogs" in the core documentation.
        /// </summary>
        /// <param name="id">ID of dialog</param>
        /// <param name="any">Object containing options to be passed into the dialog</param>
        public async void BeginDialog(string id, object any)
        {

        }

        /// <summary>
        /// Cancel any and all active dialogs for the current user/context.
        /// </summary>
        public async Task<DialogTurnResult> CancelAllDialogs()
        {
            return new DialogTurnResult(DialogTurnStatus.Empty);
        }

        /// <summary>
        /// Replace any active dialogs with a new a pre-defined dialog by specifying its id. The dialog will be started in the same context (same user, same channel) in which the original incoming message was received.
        /// See "Using Dialogs" in the core documentation.
        /// </summary>
        /// <param name="id">ID of dialog</param>
        /// <param name="options">Object containing options to be passed into the dialog</param>
        public async void ReplaceDialog(string id, object options)
        {

        }

        /// <summary>
        /// Alter the context in which a bot instance will send messages.
        /// Use this method to create or adjust a bot instance so that it can send messages to a predefined user/channel combination.
        /// </summary>
        /// <param name="reference">A ConversationReference, most likely captured from an incoming message and stored for use in proactive messaging scenarios.</param>
        public async Task<BotWorker> ChangeContext(ConversationReference reference)
        {
            return new BotWorker(new Botkit(new BotkitConfiguration()), new object());
        }


        public async void StartConversationWithUser(object reference)
        {

        }

        /// <summary>
        /// Take a crudely-formed Botkit message with any sort of field (may just be a string, may be a partial message object)
        /// and map it into a beautiful BotFramework Activity.
        /// Any fields not found in the Activity definition will be moved to activity.channelData.
        /// </summary>
        /// <param name="message">Message a string or partial outgoing message object</param>
        /// <returns>A properly formed Activity object</returns>
        public Activity EnsureMessageFormat(IBotkitMessage message)
        {
            return new Activity();
        }

        /// <summary>
        /// Set the HTTP response status code for this turn
        /// </summary>
        /// <param name="status">A valid HTTP status code like 200 202 301 500 etc</param>
        public void HTTPStatus(int status)
        {

        }

        /// <summary>
        /// Set the http response body for this turn.
        /// Use this to define the response value when the platform requires a synchronous response to the incoming webhook.
        /// </summary>
        /// <param name="any"></param>
        public void HTTPBody(object any)
        {

        }
    }
}
