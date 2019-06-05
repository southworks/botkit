// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.3.0

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.BotKit.Adapters.Slack;

namespace Microsoft.Bot.Sample.Slack.Controllers
{
    // This ASP Controller is created to handle a request. Dependency Injection will provide the Adapter and IBot
    // implementation at runtime. Multiple different IBot implementations running at different endpoints can be
    // achieved by specifying a more specific type for the bot constructor argument.
    [Route("api/messages")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly SlackAdapter Adapter;
        private readonly IBot Bot;

        public BotController(SlackAdapter adapter, IBot bot)
        {
            Adapter = adapter;
            Bot = bot;

            Adapter.Use(new SlackEventMiddleware());
        }

        
        [HttpPost]
        public async Task PostAsync()
        {
            // Delegate the processing of the HTTP POST to the adapter.
            // The adapter will invoke the bot.
            await Adapter.ProcessAsync(Request, Response, Bot);
        }
    }
}
