// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microsoft.BotKit
{
    public class BotkitBotFrameworkAdapter : BotFrameworkAdapter
    {
        protected readonly MicrosoftAppCredentials _credentials;

        /// <summary>
        /// This class extends the BotFrameworkAdapter with a few additional features to support Microsoft Teams.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="credentials"></param>
        public BotkitBotFrameworkAdapter(ICredentialProvider options, MicrosoftAppCredentials credentials) : base(options)
        {
            _credentials = credentials;
        }

        /// <summary>
        /// Get the list of channels in a MS Teams team.
        /// Can only be called with a TurnContext that originated in a team conversation - 1:1 conversations happen _outside a team_ and thus do not contain the required information to call this API.
        /// </summary>
        /// <param name="turnContext">A TurnContext object representing a message or event from a user in Teams</param>
        /// <returns>An array of channels in the format [{name: string, id: string}]</returns>
        public async Task<Channel[]> GetChannels(TurnContext turnContext)
        {
            if(turnContext.Activity.ChannelData != null && (turnContext.Activity.ChannelData as dynamic).team != null) //TO-DO: replace 'as dynamic'
            {
                var token = await _credentials.GetTokenAsync(true);

                var uri = $"{ turnContext.Activity.ServiceUrl }v3/teams/{ (turnContext.Activity.ChannelData as dynamic).team.id }/conversations/"; //TO-DO: replace 'as dynamic'

                return await RequestUrl(token, uri);
            }
            else
            {
                return new Channel[] { };
            }
        }

        public async Task<Channel[]> RequestUrl(string token, string uri)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer { token }");

                HttpResponseMessage response = await client.GetAsync(uri);

                var result = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject(result);
                var conversations = (json as dynamic).conversations; //TO-DO: replace 'as dynamic'

                if (conversations != null && conversations.count > 0)
                {
                    foreach(var c in conversations)
                    {
                        if(c.name is null)
                        {
                            c.name = "General";
                        }
                    }

                    return conversations;
                }

                return new Channel[] { };
            }
        }

        /// <summary>
        /// Allows for mocking of the connector client in unit tests.
        /// </summary>
        /// <param name="serviceUrl">Clients service url.</param>
        protected ConnectorClient CreateConnectorClient(string serviceUrl)
        {
            return new ConnectorClient(new Uri(serviceUrl), _credentials);
        }

        /// <summary>
        /// Allows for mocking of the OAuth API Client in unit tests.
        /// </summary>
        /// <param name="serviceUrl">Clients service url.</param>
        protected OAuthClient CreateTokenApiClient(string serviceUrl) 
        {
            return new OAuthClient(new Uri(serviceUrl), _credentials);
        }
    }
}
