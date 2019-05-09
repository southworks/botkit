using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotkitLibrary.Conversation
{
    /// <summary>
    /// Definition of the handler functions used to handle .ask and .addQuestion conditions
    /// </summary>
    public interface IBotkitConvoHandler
    {
        Func<string, BotkitDialogWrapper, BotWorker, object> MyProperty { get; set; }
    }
}
