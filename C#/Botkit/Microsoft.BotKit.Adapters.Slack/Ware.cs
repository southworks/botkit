// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.BotKit.Adapters.Slack
{
    public class Ware
    {
        /// <summary>
        /// Valid names: spawn, ingest, send, receive
        /// </summary>
        public string Name;
        public List<Action<BotWorker, Action>> Middlewares;
    }
}
