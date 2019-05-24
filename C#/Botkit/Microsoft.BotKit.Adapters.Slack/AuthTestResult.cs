// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using SlackAPI;

namespace Microsoft.BotKit.Adapters.Slack
{
    /// <summary>
    /// Interface to cast result of web api calls
    /// </summary>
    public abstract class AuthTestResult : Response
    {
        string user { get; }
        string team { get; }
        string userId { get; }
        string teamId { get; }
    }
}
