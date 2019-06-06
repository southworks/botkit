// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.
using SlackAPI;
using SlackAPI.RPCMessages;

namespace Microsoft.BotKit.Adapters.Slack
{
    /// <summary>
    /// Abstract class to cast result of web api calls
    /// </summary>
    public class ChatPostEphemeralMessageResult : PostEphemeralResponse
    {
        public string Id { get; }
    }
}
