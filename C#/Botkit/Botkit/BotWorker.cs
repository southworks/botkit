using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotkitLibrary.Core;

namespace BotkitLibrary
{
    public class BotWorker
    {
        private Botkit Controller { get; set; }

        public BotWorker(Botkit controller)
        {
            Controller = controller;
        }

        public Botkit GetController()
        {
            return Controller;
        }

    }
}
