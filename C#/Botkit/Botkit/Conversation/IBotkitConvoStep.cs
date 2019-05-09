using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotkitLibrary.Conversation
{
    public interface IBotkitConvoStep
    {
        /// <summary>
        /// The number pointing to the current message in the current thread in this dialog's script
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// The name of the current thread
        /// </summary>
        string Thread { get; set; }

        /// <summary>
        /// A pointer to the current dialog state
        /// </summary>
        object State { get; set; }

        /// <summary>
        /// A pointer to any options passed into the dialog when it began
        /// </summary>
        object Options { get; set; }

        /// <summary>
        /// The reason for this step being called
        /// </summary>
        DialogReason Reason { get; set; }

        /// <summary>
        /// The results of the previous turn
        /// </summary>
        object Result { get; set; }

        /// <summary>
        /// A pointer directly to state.values
        /// </summary>
        object Values { get; set; }

        /// <summary>
        /// A function to call when the step is completed.
        /// </summary>
        Func<object, object> Next { get; set; }
    }
}
