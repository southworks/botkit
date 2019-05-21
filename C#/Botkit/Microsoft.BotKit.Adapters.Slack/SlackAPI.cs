// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SlackAPI;

namespace Microsoft.BotKit.Adapters.Slack
{
    public class SlackAPI
    {
        private readonly string Token;
        private SlackSocketClient client;

        public SlackAPI(string token)
        {
            Token = token;
            Initiate();
        }

        private void Initiate()
        {
            ManualResetEventSlim clientReady = new ManualResetEventSlim(false);
            client = new SlackSocketClient(Token);
            client.Connect((connected) =>
            {
                // "This is called once the client has emitted the RTM start command"
                clientReady.Set();
            }, () =>
            {
                // "This is called once the RTM client has connected to the end point"
            });

            client.OnMessageReceived += (message) =>
            {
                //Console.WriteLine(message);
            };
            clientReady.Wait();
        }

        public string GetIdentity()
        {
            return client.MySelf != null
                ? client.MySelf.id
                : throw new Exception("Invalid credentials have been provided and the bot can't start");
        }
    }
}
