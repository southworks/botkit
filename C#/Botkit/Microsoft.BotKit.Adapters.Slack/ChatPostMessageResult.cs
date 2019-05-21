// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.BotKit.Adapters.Slack
{
    /// <summary>
    /// Abstract class to cast result of web api calls
    /// </summary>
    public abstract class ChatPostMessageResult
    {
        string channel { get; }
        string ts { get; }
        string message { get; }
    }
}
