// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.BotKit.Core
{
    public class Middleware
    {
        public IMiddleware Spawn { get; set; }
        public IMiddleware Ingest { get; set; }
        public IMiddleware Send { get; set; }
        public IMiddleware Receive { get; set; }
    }
}
