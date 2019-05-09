using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotkitLibrary.Core
{
    public class Middleware
    {
        public IMiddleware Spawn { get; set; }
        public IMiddleware Ingest { get; set; }
        public IMiddleware Send { get; set; }
        public IMiddleware Receive { get; set; }
    }
}
