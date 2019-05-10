using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BotkitLibrary.Core
{
    public interface IBotkitTrigger
    {
        string Type { get; set; }
        string Pattern { get; set; }
        Regex RegEx { get; set; }
        Tuple<IBotkitMessage, Task<bool>> Message { get; set; }
        IBotkitHandler Handler { get; set; }
    }
}
