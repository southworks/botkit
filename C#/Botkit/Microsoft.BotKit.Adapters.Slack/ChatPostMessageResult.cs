// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.
using SlackAPI;

namespace Microsoft.BotKit.Adapters.Slack
{
    /// <summary>
    /// Abstract class to cast result of web api calls
    /// </summary>
    public class ChatPostMessageResult : PostMessageResponse
    {
        public string Id { get; }
        public string Channel { get; }
        public string Ts { get; }
    }
}
