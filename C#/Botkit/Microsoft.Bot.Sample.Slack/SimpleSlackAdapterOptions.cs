using Microsoft.Bot.Connector.Authentication;
using Microsoft.BotKit.Adapters.Slack;
using System.Threading.Tasks;

namespace Microsoft.Bot.Sample.Slack
{
    public class SimpleSlackAdapterOptions : ISlackAdapterOptions
    {
        public SimpleSlackAdapterOptions()
        {
        }

        public SimpleSlackAdapterOptions(string verificationToken, string botToken)
        {
            VerificationToken = verificationToken;
            BotToken = botToken;
        }

        public string VerificationToken { get; set; }
        public string ClientSigningSecret { get; set; }
        public string BotToken { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] Scopes { get; set; }
        public string RedirectUri { get; set; }

        public Task<string> GetBotUserByTeam(string TeamId)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetTokenForTeam(string TeamId)
        {
            throw new System.NotImplementedException();
        }
    }
}
