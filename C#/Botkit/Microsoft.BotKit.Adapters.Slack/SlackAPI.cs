// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using SlackAPI;
using SlackAPI.RPCMessages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.BotKit.Adapters.Slack
{
    public class SlackAPI
    {
        private readonly string Token;
        private SlackTaskClient client;

        public SlackAPI(string token)
        {
            Token = token;
            Initiate();
        }

        private void Initiate()
        {
            ManualResetEventSlim clientReady = new ManualResetEventSlim(false);
            
            client = new SlackTaskClient(Token);
            client.ConnectAsync();
        }

        public string GetIdentity()
        {
            return client.MySelf != null
                ? client.MySelf.id
                : throw new Exception("Invalid credentials have been provided and the bot can't start");
        }

        public Task<DeletedResponse> DeleteMessage(string channelId, DateTime ts)
        {
            return client.DeleteMessageAsync(channelId, ts);
        }

        public Task<DialogOpenResponse> DialogOpen(string triggerId, Dialog dialog)
        {
            return client.DialogOpenAsync(triggerId, dialog);
        }

        public Task<AccessTokenResponse> GetAccessToken(string clientId, string clientSecret, string redirectUri, string code)
        {
            var helpers = new SlackClientHelpers();
            return helpers.GetAccessTokenAsync(clientId, clientSecret, redirectUri, code);
        }

        public Task<JoinDirectMessageChannelResponse> JoinDirectMessageChannel(string user)
        {
            return client.JoinDirectMessageChannelAsync(user);
        }

        public Task<PostEphemeralResponse> PostEphemeralMessage(string channelId, string text, string targetUser)
        {
            return client.PostEphemeralMessageAsync(channelId, text, targetUser);
        }

        public Task<PostMessageResponse> PostMessage(string channelId, string text)
        {
            return client.PostMessageAsync(channelId, text);
        }

        public Task<AuthTestResponse> TestAuth()
        {
            return client.TestAuthAsync();
        }

        public Task<UpdateResponse> Update(string ts, string channelId, string text)
        {
            return client.UpdateAsync(ts, channelId, text);
        }
    }
}
