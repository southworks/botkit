using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotkitLibrary.Core
{
    public interface IBotkitHandler
    {
        public Func<BotWorker, IBotkitMessage, object> Handler { get; set; }
    }
}
