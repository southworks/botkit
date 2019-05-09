using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BotkitLibrary.Conversation
{
    /// <summary>
    /// Definition of the trigger pattern passed into .ask or .addQuestion
    /// </summary>
    public interface IBotkitConvoTrigger
    {
        string Type { get; set; }
        Regex Pattern { get; set; }
        IBotkitConvoHandler Handler { get; set; }
        bool Default { get; set; }
    }
}
