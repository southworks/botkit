using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotkitLibrary.Core
{
    public interface IBotkitPlugin
    {
        string Name { get; set; }
        Action<Botkit> Init { get; set; }
        Tuple<string, object[]> Middlewares { get; set; }
    }
}
