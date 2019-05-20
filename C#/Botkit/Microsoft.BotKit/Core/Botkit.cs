// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.BotKit.Core
{
    public class Botkit
    {
        public string Version = "Not yet implemented";

        /// <summary>
        /// Middleware endpoints available for plugins and features to extend Botkit.
        /// Endpoints available are: spawn, ingest, receive, send.
        /// </summary>
        public Middleware Middleware { get; set; }

        /// <summary>
        /// A BotBuilder storage driver - defaults to MemoryStorage
        /// </summary>
        public IStorage Storage { get; set; }

        /// <summary>
        /// An Express webserver
        /// </summary>
        public object Webserver { get; set; }

        /// <summary>
        /// A direct reference to the underlying HTTP server object
        /// </summary>
        public object HTTP { get; set; }

        /// <summary>
        /// Any BotBuilder-compatible adapter - defaults to a BotFrameworkAdapter
        /// </summary>
        public object Adapter { get; set; }

        /// <summary>
        /// A BotBuilder DialogSet that serves as the top level dialog container for the Botkit app
        /// </summary>
        public DialogSet DialogSet { get; set; }

        /// <summary>
        /// The path of the main Botkit SDK, used to generate relative paths
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Create a new Botkit instance and optionally specify a platform-specific adapter.
        /// By default, Botkit will create a BotFrameworkAdapter.
        /// </summary>
        /// <param name="configuration">Configuration for this instance of Botkit</param>
        public Botkit(BotkitConfiguration configuration)
        {

        }

        /// <summary>
        /// Shutdown the webserver and prepare to terminate the app.
        /// Causes Botkit to first emit a special shutdown event, process any bound handlers, and then finally terminate the webserver.
        /// Bind any necessary cleanup helpers to the shutdown event - for example, close the connection to mongo.
        /// </summary>
        public async void Shutdown()
        {

        }

        /// <summary>
        /// Get a value from the configuration.
        /// </summary>
        /// <param name="key">The name of a value stored in the configuration</param>
        /// <returns>The value stored in the configuration (or null if absent)</returns>
        public object GetConfig(string key)
        {
            return new object();
        }

        /// <summary>
        /// Load a plugin module and bind all included middlewares to their respective endpoints.
        /// </summary>
        /// <param name="plugin">A plugin module.</param>
        public void UsePlugin(IBotkitPlugin plugin)
        {

        }

        /// <summary>
        /// (Plugins only) Extend Botkit's controller with new functionality and make it available globally via the controller object.
        /// </summary>
        /// <param name="name">Name of plugin</param>
        /// <param name="extension">An object containing methods</param>
        public void AddPluginExtension(string name, object extension)
        {

        }

        /// <summary>
        /// Access plugin extension methods.
        /// After a plugin calls controller.addPluginExtension('foo', extension_methods), the extension will then be available at controller.plugins.foo
        /// </summary>
        public Tuple<string, object> Plugins { get; }

        /// <summary>
        /// Expose a folder to the web as a set of static files.
        /// Useful for plugins that need to bundle additional assets!
        /// </summary>
        /// <param name="alias">The public alias ie /myfiles</param>
        /// <param name="path">The actual path something like __dirname + '/public'</param>
        public void PublicFolder(string alias, string path)
        {

        }

        /// <summary>
        /// Convert a local path from a plugin folder to a full path relative to the webserver's main views folder.
        /// Allows a plugin to bundle views/layouts and make them available to the webserver's renderer.
        /// </summary>
        /// <param name="pathToView">something like Path.Combine()</param>
        /// <returns></returns>
        public string GetLocalView(string pathToView)
        {
            return "";
        }


        /// <summary>
        /// (For use by Botkit plugins only) - Add a dependency to Botkit's bootup process that must be marked as completed using completeDep().
        /// Botkit's controller.ready() function will not fire until all dependencies have been marked complete.
        /// </summary>
        /// <param name="name"></param>
        public void AddDep(string name)
        {

        }

        /// <summary>
        /// (For use by plugins only) - Mark a bootup dependency as loaded and ready to use
        /// Botkit's controller.ready() function will not fire until all dependencies have been marked complete.
        /// </summary>
        /// <param name="name">{string} The name of the dependency that has completed loading.</param>
        public bool CompleteDep(string name)
        {
            return false;
        }

        /// <summary>
        /// Use controller.ready() to wrap any calls that require components loaded during the bootup process.
        /// This will ensure that the calls will not be made until all of the components have successfully been initialized.
        /// </summary>
        /// <param name="handler">A function to run when Botkit is booted and ready to run.</param>
        public void Ready(Task<object> handler)
        {

        }

        /// <summary>
        /// Accepts the result of a BotBuilder adapter's processActivity() method and processes it into a Botkit-style message and BotWorker instance
        /// which is then used to test for triggers and emit events.
        /// NOTE: This method should only be used in custom adapters that receive messages through mechanisms other than the main webhook endpoint (such as those received via websocket, for example)
        /// </summary>
        /// <param name="turnContext">A TurnContext representing an incoming message, typically created by an adapter's processActivity() method.</param>
        /// <returns></returns>
        public async Task<object> HandleTurn(TurnContext turnContext)
        {
            return new object();
        }

        /// <summary>
        /// Save the current conversation state pertaining to a given BotWorker's activities.
        /// Note: this is normally called internally and is only required when state changes happen outside of the normal processing flow.
        /// </summary>
        /// <param name="bot"></param>
        public async void SaveState(BotWorker bot)
        {

        }

        /// <summary>
        /// Instruct your bot to listen for a pattern, and do something when that pattern is heard.
        /// Patterns will be "heard" only if the message is not already handled by an in-progress dialog.
        /// To "hear" patterns _before_ dialogs are processed, use `controller.interrupts()` instead.
        /// </summary>
        /// <param name="patterns">One or more string, regular expression, or test function</param>
        /// <param name="events">A list of event types that should be evaluated for the given patterns</param>
        /// <param name="handler">A function that will be called should the pattern be matched</param>
        public void Hears(Regex patterns, string[] events, IBotkitHandler handler)
        {

        }

        /// <summary>
        /// Instruct your bot to listen for a pattern, and do something when that pattern is heard.
        /// Interruptions work just like "hears" triggers, but fire _before_ the dialog system is engaged,
        /// and thus handlers will interrupt the normal flow of messages through the processing pipeline.
        /// </summary>
        /// <param name="patterns"></param>
        /// <param name="events"></param>
        /// <param name="handler"></param>
        public void Interrupts(Regex patterns, string[] events, IBotkitHandler handler)
        {

        }

        /// <summary>
        /// Bind a handler function to one or more events.
        /// </summary>
        /// <param name="events">One or more event names</param>
        /// <param name="handler">A handler function that will fire whenever one of the named events is received.</param>
        public void On(string[] events, IBotkitHandler handler)
        {

        }

        /// <summary>
        /// Trigger an event to be fired.  This will cause any bound handlers to be executed.
        /// Note: This is normally used internally, but can be used to emit custom events.
        /// </summary>
        /// <param name="eventName">The name of the event</param>
        /// <param name="bot">A BotWorker instance created using controller.spawn()</param>
        /// <param name="botkit">An incoming message or event</param>
        /// <returns></returns>
        public async Task<object> Trigger(string eventName, BotWorker bot, IBotkitMessage botkit)
        {
            return new object();
        }

        /// <summary>
        /// Create a platform-specific BotWorker instance that can be used to respond to messages or generate new outbound messages.
        /// The spawned bot contains all information required to process outbound messages and handle dialog state, and may also contain extensions
        /// for handling platform-specific events or activities.
        /// </summary>
        /// <param name="config">Preferably receives a DialogContext, though can also receive a TurnContext. If excluded, must call bot.changeContext(reference) before calling any other method.</param>
        /// <returns></returns>
        public async Task<BotWorker> Spawn(object config)
        {
            return new BotWorker(new Botkit(new BotkitConfiguration()), config);
        }

        /// <summary>
        /// Load a Botkit feature module
        /// </summary>
        /// <param name="path">Path to module file</param>
        public void LoadModule(string path)
        {

        }

        /// <summary>
        /// Load all Botkit feature modules located in a given folder.
        /// </summary>
        /// <param name="path">Path to a folder of module files</param>
        public void LoadModules(string path)
        {

        }

        /// <summary>
        /// Add a dialog to the bot, making it accessible via bot.beginDialog(dialog_id)
        /// </summary>
        /// <param name="dialog">A dialog to be added to the bot's dialog set</param>
        public void AddDialog(Dialog dialog)
        {

        }

        /// <summary>
        /// Bind a handler to the end of a dialog.
        /// NOTE: bot worker cannot use bot.reply(), must use bot.send()
        /// </summary>
        /// <param name="dialog">The dialog object or the id of the dialog</param>
        /// <param name="handler">A handler function in the form async(bot, dialog_results) => {}</param>
        public void AfterDialog(Dialog dialog, IBotkitHandler handler)
        {

        }
    }
}
