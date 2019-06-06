// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.3.0

using Microsoft.Extensions.Configuration;

namespace Microsoft.Bot.Sample.Slack
{
    public class ConfigurationSlackAdapterOptions : SimpleSlackAdapterOptions
    {
        public ConfigurationSlackAdapterOptions(IConfiguration configuration)
             : base(configuration["MicrosoftAppId"], configuration["MicrosoftAppPassword"])
        {
            BotToken = "";
            VerificationToken = "";
        }
    }
}
