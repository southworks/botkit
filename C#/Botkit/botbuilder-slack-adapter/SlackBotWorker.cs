using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotkitLibrary;
using BotkitLibrary.Core;

namespace botbuilder_slack_adapter
{
    public class SlackBotWorker : BotWorker
    {
        /// <summary>
        /// Reserved for use internally by Botkit's `controller.spawn()`, this class is used to create a BotWorker instance that can send messages, replies, and make other API calls.
        /// When used with the SlackAdapter's multi-tenancy mode, it is possible to spawn a bot instance by passing in the Slack workspace ID of a team that has installed the app.
        /// Use this in concert with [startPrivateConversation()](#startPrivateConversation) and [changeContext()](core.md#changecontext) to start conversations
        /// or send proactive alerts to users on a schedule or in response to external events.
        /// </summary>
        /// <param name="botkit">The Botkit controller object responsible for spawning this bot worker</param>
        /// <param name="config">Normally, a DialogContext object.  Can also be the id of a team.</param>
        public SlackBotWorker(Botkit botkit, object config) : base(botkit, config)
        {

        }

        /// <summary>
        /// Switch a bot's context to a 1:1 private message channel with a specific user.
        /// After calling this method, messages sent with `bot.say` and any dialogs started with `bot.beginDialog` will occur in this new context.
        /// </summary>
        /// <param name="userId">A Slack user id, like one found in `message.user` or in a `<@mention>`</param>
        public async Task<object> StartPrivateConversation(string userId)
        {
            return new object();
        }

        /// <summary>
        /// Switch a bot's context into a different channel.
        /// After calling this method, messages sent with `bot.say` and any dialogs started with `bot.beginDialog` will occur in this new context.
        /// </summary>
        /// <param name="channelId">A Slack channel id, like one found in `message.channel`</param>
        /// <param name="userId">A Slack user id, like one found in `message.user` or in a `<@mention>`</param>
        public async Task<object> StartConversationInChannel(string channelId, string userId)
        {
            return new object();
        }

        /// <summary>
        /// Switch a bot's context into a specific sub-thread within a channel.
        /// After calling this method, messages sent with `bot.say` and any dialogs started with `bot.beginDialog` will occur in this new context.
        /// </summary>
        /// <param name="channelId">A Slack channel id, like one found in `message.channel`</param>
        /// <param name="userId">A Slack user id, like one found in `message.user` or in a `<@mention>`</param>
        /// <param name="threadTs">A thread_ts value found in the `message.thread_ts` or `message.ts` field.</param>
        public async Task<object> StartConversationInThread(string channelId, string userId, string threadTs)
        {
            return new object();
        }

        /// <summary>
        /// Like bot.reply, but as a threaded response to the incoming message rather than a new message in the main channel.
        /// </summary>
        /// <param name="source">An incoming message object</param>
        /// <param name="resp">An outgoing message object (or part of one or just reply text)</param>
        public async Task<object> ReplyInThread(object source, object resp)
        {
            return new object();
        }

        /// <summary>
        /// Like bot.reply, but sent as an "ephemeral" message meaning only the recipient can see it.
        /// Uses chat.postEphemeral
        /// </summary>
        /// <param name="source">An incoming message object</param>
        /// <param name="resp">An outgoing message object (or part of one or just reply text)</param>
        public async Task<object> ReplyEphemeral(object source, object resp)
        {
            return new object();
        }

        /// <summary>
        /// Like bot.reply, but used to send an immediate public reply to a /slash command.
        /// The message in `resp` will be displayed to everyone in the channel.
        /// </summary>
        /// <param name="source">An incoming message object of type `slash_command`</param>
        /// <param name="resp">An outgoing message object (or part of one or just reply text)</param>
        public async Task<object> ReplyPublic(object source, object resp)
        {
            return new object();
        }

        /// <summary>
        /// Like bot.reply, but used to send an immediate private reply to a /slash command.
        /// The message in `resp` will be displayed only to the person who executed the slash command.
        /// </summary>
        /// <param name="source">An incoming message object of type `slash_command`</param>
        /// <param name="resp">An outgoing message object (or part of one or just reply text)</param>
        public async Task<object> ReplyPrivate(object source, object resp)
        {
            return new object();
        }

        /// <summary>
        /// Like bot.reply, but used to respond to an `interactive_message` event and cause the original message to be replaced with a new one.
        /// An incoming message object of type `interactive_message`
        /// </summary>
        /// <param name="source">An incoming message object of type `interactive_message`</param>
        /// <param name="resp">A new or modified message that will replace the original one</param>
        public async Task<object> ReplyInteractive(object source, object resp)
        {
            return new object();
        }

        /// <summary>
        /// Return 1 or more error to a `dialog_submission` event that will be displayed as form validation errors.
        /// Each error must be mapped to the name of an input in the dialog.
        /// </summary>
        /// <param name="error">1 or more objects in form {name: string, error: string}</param>
        public void DialogError(object error)
        {

        }

        /// <summary>
        /// Reply to a button click with a request to open a dialog.
        /// </summary>
        /// <param name="source">An incoming `interactive_callback` event containing a `trigger_id` field</param>
        /// <param name="dialogObj">A dialog, as created using [SlackDialog](#SlackDialog) or [authored to this spec](https://api.slack.com/dialogs).</param>
        public async Task<object> ReplyWithDialog(object source, object dialogObj)
        {
            return new object();
        }

        /// <summary>
        /// Update an existing message with new content.
        /// </summary>
        /// <param name="update">An object in the form `{id: <id of message to update>, conversation: { id: <channel> }, text: <new text>, card: <array of card objects>}`</param>
        public async Task<object> UpdateMessage(IBotkitMessage update)
        {
            return new object();
        }

        /// <summary>
        /// Delete an existing message.
        /// </summary>
        /// <param name="update">An object in the form of `{id: <id of message to delete>, conversation: { id: <channel of message> }}`</param>
        public async Task<object> DeleteMessage(IBotkitMessage update)
        {
            return new object();
        }
    }
}
