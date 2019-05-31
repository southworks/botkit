// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Bot.Builder;

namespace Microsoft.BotKit.Core
{
    public class BotkitConfiguration
    {
        public string WebhookUri { get; set; }
        public string DialogStateProperty { get; set; }
        public BotkitBotFrameworkAdapter Adapter { get; set; } // TO-DO: compare with TS implementation
        public Tuple<AdapterKey, string> AdapterConfig { get; set; }
        public IWebserver Webserver { get; set; }
        public IStorage Storage { get; set; }
        public bool DisableWebserver { get; set; }
    }

    public interface IStoreItems
    {
        string Key { get; set; }
    }

    public interface IWebserver
    {

    }

    public enum AdapterKey
    {
        AppID,
        AppPassword,
        ChannelAuthTenant,
        ChannelService,
        oAuthEndpoint,
        openIdMetadata
    }
}
