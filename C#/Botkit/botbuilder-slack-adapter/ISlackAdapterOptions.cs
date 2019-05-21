using System.Threading.Tasks;

namespace botbuilder_slack_adapter
{
    public interface ISlackAdapterOptions
    {
        /// <summary>
        /// Legacy method for validating the origin of incoming webhooks.
        /// </summary>
        string VerificationToken { get; }

        /// <summary>
        /// A token used to validate that incoming webhooks originated with Slack.
        /// </summary>
        string ClientSigningSecret { get; }

        /// <summary>
        /// A token (provided by Slack) for a bot to work on a single workspace.
        /// </summary>
        string BotToken { get; }

        /// <summary>
        /// The oauth client id provided by Slack for multi-team apps.
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// The oauth client secret provided by Slack for multi-team apps.
        /// </summary>
        string ClientSecret { get; }

        /// <summary>
        /// A an array of scope names that are being requested during the oauth process. Must match the scopes defined at api.slack.com.
        /// </summary>
        string[] Scopes { get; }

        /// <summary>
        /// The URL users will be redirected to after an oauth flow. In most cases, should be `https://<mydomain.com>/install/auth`.
        /// </summary>
        string RedirectUri { get; }

        /// <summary>
        /// A method that receives a Slack team id and returns the bot token associated with that team. Required for multi-team apps.
        /// </summary>
        /// <param name="TeamId">Team ID</param>
        /// <returns></returns>
        Task<string> GetTokenForTeam(string TeamId);

        /// <summary>
        /// A method that receives a Slack team id and returns the bot user id associated with that team. Required for multi-team apps.
        /// </summary>
        /// <param name="TeamId">Team ID.</param>
        /// <returns></returns>
        Task<string> GetBotUserByTeam(string TeamId);
    }
}
