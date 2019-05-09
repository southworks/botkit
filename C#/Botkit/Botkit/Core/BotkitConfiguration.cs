using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adapters;
using Microsoft.Bot.Builder;

namespace BotkitLibrary.Core
{
    public class BotkitConfiguration
    {
        public string WebhookUri { get; set; }
        public string DialogStateProperty { get; set; }
        public Adapter Adapter { get; set; }
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
