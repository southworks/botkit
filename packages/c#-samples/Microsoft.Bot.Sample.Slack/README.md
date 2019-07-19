# Microsoft.Bot.Sample.Slack

Bot Framework v4 echo bot using Slack Adapter sample.

This bot has been created using [Bot Framework](https://dev.botframework.com), it shows how to create a simple bot and connect it to Slack using the Slack Adapter.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) version 2.1 or higher

  ```bash
  # determine dotnet version
  dotnet --version
  ```

## To try this sample

- Create a **Slack Application** for the bot following [this guide](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-channel-connect-slack?view=azure-bot-service-4.0)

- Complete the Slack credentials (_VerificationToken_ and _BotToken_) in [appsettings.json](https://github.com/southworkscom/botbuilder-dotnet/blob/add/botkit-packages/libraries/BotKit%20Samples/Microsoft.Bot.Sample.Slack/appsettings.json)

- In a terminal, navigate to `Microsoft.Bot.Sample.Slack`

    ```bash
    # change into project folder
    cd # Microsoft.Bot.Sample.Slack
    ```

- Run the bot from a terminal or from Visual Studio, choose option A or B.

  A) From a terminal

  ```bash
  # run the bot
  dotnet run
  ```

  B) Or from Visual Studio

  - Launch Visual Studio
  - File -> Open -> Project/Solution
  - Navigate to `Microsoft.Bot.Sample.Slack` folder
  - Select `Microsoft.Bot.Sample.Slack.csproj` file
  - Press `F5` to run the project

- Using [Ngrok](https://ngrok.com/) tool, run the following command to create a public HTTPS URL for your localhost 
```bash
  # expose your localhost
  ngrok http 3978 -host-header="localhost:3978"
  ```
- Configure this URL in your Slack Application

- Browse for your App in Slack and send a message to your bot 

## Deploy the bot to Azure

To learn more about deploying a bot to Azure, see [Deploy your bot to Azure](https://aka.ms/azuredeployment) for a complete list of deployment instructions.

## Further reading

- [Bot Framework Documentation](https://docs.botframework.com)
- [Bot Basics](https://docs.microsoft.com/azure/bot-service/bot-builder-basics?view=azure-bot-service-4.0)
- [Activity processing](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-concept-activity-processing?view=azure-bot-service-4.0)
- [Azure Bot Service Introduction](https://docs.microsoft.com/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0)
- [Azure Bot Service Documentation](https://docs.microsoft.com/azure/bot-service/?view=azure-bot-service-4.0)
- [.NET Core CLI tools](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x)
- [Azure CLI](https://docs.microsoft.com/cli/azure/?view=azure-cli-latest)
- [Azure Portal](https://portal.azure.com)
- [Language Understanding using LUIS](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/)
- [Channels and Bot Connector Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-concepts?view=azure-bot-service-4.0)
