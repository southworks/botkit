// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

const restify = require('restify');

const { ActivityHandler, MemoryStorage, UserState, ConversationState, InspectionState, InspectionMiddleware } = require('botbuilder');

const { FacebookAdapter } = require('botbuilder-adapter-facebook')

// Facebook Adapter
const adapter = new FacebookAdapter({
    verify_token: '',
    app_secret: '',
    access_token: ''
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

server.listen(process.env.port || process.env.PORT || 3978, () => {
    console.log(`\n${ server.name } listening to ${ server.url }`);
    console.log(`\nGet Bot Framework Emulator: https://aka.ms/botframework-emulator`);
    console.log(`\nTo talk to your bot, open the emulator select "Open Bot"`);
});

// Listen for incoming requests.
server.get('/api/messages', (req, res) => {
    adapter.processActivity(req, res, async( context ) => {
        await bot.run(context);
    });
});

server.post('/api/messages', (req, res) => {
    adapter.processActivity(req, res, async( context ) => {
        await bot.run(context);
    });
});