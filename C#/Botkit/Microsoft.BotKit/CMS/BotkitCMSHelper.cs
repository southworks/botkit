// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.BotKit.Core;
using Microsoft.Bot.Builder.Dialogs;

namespace Microsoft.BotKit.CMS
{
    public class BotkitCMSHelper
    {
        public BotkitCMSHelper(Botkit botkit, CMSConfiguration configuration)
        {

        }

        /// <summary>
        /// Load all script content from the configured CMS instance into a DialogSet and prepare them to be used.
        /// </summary>
        /// <param name="dialogSet">A DialogSet into which the dialogs should be loaded.  In most cases, this is `controller.dialogSet`, allowing Botkit to access these dialogs through `bot.beginDialog()`.</param>
        public async void LoadAllScripts(DialogSet dialogSet)
        {

        }

        /// <summary>
        /// Uses the Botkit CMS trigger API to test an incoming message against a list of predefined triggers.
        /// If a trigger is matched, the appropriate dialog will begin immediately.
        /// </summary>
        /// <param name="bot">The current bot worker instance</param>
        /// <param name="message">An incoming message to be interpreted</param>
        public async void TestTrigger(BotWorker bot, IBotkitMessage message)
        {

        }

        /// <summary>
        /// Bind a handler function that will fire before a given script and thread begin.
        /// Provides a way to use BotkitConversation.before() on dialogs loaded dynamically via the CMS api instead of being created in code.
        /// </summary>
        /// <param name="scriptName">The name of the script to bind to</param>
        /// <param name="threadName">The name of a thread within the script to bind to</param>
        /// <param name="handler">A handler function in the form async(convo, bot) => {}</param>
        public void Before(string scriptName, string threadName, Action<BotkitDialogWrapper, BotWorker> handler)
        {
            // TO-DO: the method firm must match this:
            // public before(script_name: string, thread_name: string, handler: (convo: BotkitDialogWrapper, bot: BotWorker) => Promise<void>): void {
        }

        /// <summary>
        /// Bind a handler function that will fire when a given variable is set within a a given script.
        /// Provides a way to use BotkitConversation.onChange() on dialogs loaded dynamically via the CMS api instead of being created in code.
        /// </summary>
        /// <param name="scriptName">The name of the script to bind to</param>
        /// <param name="variableName">The name of a variable within the script to bind to</param>
        /// <param name="">A handler function in the form async(value, convo, bot) => {}</param>
        public void OnChange(string scriptName, string variableName, Action<object, BotWorker> handler)
        {
            // TO-DO: Method name must match this:
            // public onChange(script_name: string, variable_name: string, handler: (value: any, convo: BotkitDialogWrapper, bot: BotWorker) => Promise<void>): void {
        }

        /// <summary>
        /// Bind a handler function that will fire after a given dialog ends.
        /// Provides a way to use BotkitConversation.after() on dialogs loaded dynamically via the CMS api instead of being created in code.
        /// </summary>
        /// <param name="scriptName">The name of the script to bind to</param>
        /// <param name="handler">A handler function in the form async(results, bot) => {}</param>
        public void After(string scriptName, Action<object, BotWorker> handler)
        {
            // TO-DO: method name must match this:
            // public after(script_name: string, handler: (results: any, bot: BotWorker) => Promise<void>): void {
        }
    }
}
