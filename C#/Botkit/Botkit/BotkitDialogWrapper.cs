using BotkitLibrary.Conversation;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotkitLibrary
{
    public class BotkitDialogWrapper
    {
        /// <summary>
        /// An object containing variables and user responses from this conversation.
        /// </summary>
        public Tuple<string, object> Vars { get; set; }

        public BotkitDialogWrapper(DialogContext dialogContext, IBotkitConvoStep botkitconvoStep)
        {

        }

        /// <summary>
        /// Jump immediately to the first message in a different thread.
        /// </summary>
        /// <param name="thread">Name of a thread</param>
        public async void GotoThread(string thread)
        {

        }

        /// <summary>
        ///  Repeat the last message sent on the next turn.
        /// </summary>
        public async void Repeat()
        {

        }

        /// <summary>
        /// Set the value of a variable that will be available to messages in the conversation.
        /// Equivalent to convo.vars.key = val;
        /// Results in {{vars.key}} being replaced with the value in val.
        /// </summary>
        /// <param name="key">The name of the variable</param>
        /// <param name="val">The value for the variable</param>
        public void SetVar(object key, object val)
        {

        }
    }
}
