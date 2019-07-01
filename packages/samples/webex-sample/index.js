// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

const restify = require('restify');

const { ActivityHandler, MemoryStorage, UserState, ConversationState, InspectionState, InspectionMiddleware } = require('botbuilder');

const { WebexAdapter } = require('botbuilder-adapter-webex')

// Webex Adapter
const adapter = new WebexAdapter({
    access_token: process.env.WEBEX_ACCESS_TOKEN, //access token from https://developer.webex.com
    public_address: process.env.WEBEX_PUBLIC_ADDRESS, // public url of this app https://myapp.com/
    secret: 'random-secret-1234' // webhook validation secret - you can define this yourself
});

var memoryStorage = new MemoryStorage();
var inspectionState = new InspectionState(memoryStorage);

var userState = new UserState(memoryStorage);
var conversationState = new ConversationState(memoryStorage);

var conversationStateAccessor = conversationState.createProperty('test');

adapter.use(new InspectionMiddleware(inspectionState, userState, conversationState));

adapter.onTurnError = async (context, error) => {
    console.error(`\n [onTurnError]: ${ error }`);
    await context.sendActivity(`Oops. Something went wrong!`);
    conversationState.clear(context);
};

class TestBot extends ActivityHandler {
    constructor() {
        super();
        this.onMessage(async (context, next) => {

            var state = await conversationStateAccessor.get(context, { count: 0 });

            await context.sendActivity(`you said "${ context.activity.text }" ${ state.count }`);

            state.count++;
            await conversationState.saveChanges(context, false);

            await next();
        });
        this.onMembersAdded(async (context, next) => {
            const membersAdded = context.activity.membersAdded;
            for (let cnt = 0; cnt < membersAdded.length; cnt++) {
                if (membersAdded[cnt].id !== context.activity.recipient.id) {
                    await context.sendActivity(`welcome ${ membersAdded[cnt].name }`);
                }
            }
            await next();
        });        
    }
}

var bot = new TestBot(); 

console.log('welcome to test bot - a local test tool for working with the emulator');

// set up restify...
const server = restify.createServer();
server.use(restify.plugins.bodyParser());

// register the webhook subscription to start receiving messages - Botkit does this automatically!
adapter.registerWebhookSubscription('/api/messages');

// Load up the bot's identity, otherwise it won't know how to filter messages from itself
adapter.getIdentity();

server.listen(process.env.port || process.env.PORT || 3978, () => {
    console.log(`\n${ server.name } listening to ${ server.url }`);
    console.log(`\nGet Bot Framework Emulator: https://aka.ms/botframework-emulator`);
    console.log(`\nTo talk to your bot, open the emulator select "Open Bot"`);
});

// Listen for incoming requests.
server.post('/api/messages', (req, res) => {
    adapter.processActivity(req, res, async( context ) => {
        await bot.run(context);
    });
});