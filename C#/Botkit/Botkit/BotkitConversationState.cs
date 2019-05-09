using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace BotkitLibrary
{
    internal class BotkitConversationState : ConversationState
    {
        public BotkitConversationState(IStorage storage) : base(storage)
        {

        }

        public string GetStorageKey(TurnContext context)
        {
            Activity activity = context.Activity;
            string ChannelId = activity.ChannelId;

            if (activity.Conversation == null || activity.Conversation.Id == null)
            {
                throw new Exception("missing activity.conversation");
            }

            // create a combo key by sorting all the fields in the conversation address and combining them all
            // mix in user id as well, because conversations are between the bot and a single user
            const string ConversationId = "";//Object.keys(activity.conversation).sort().map((key) => activity.conversation[key]).filter((val) => val !== '' && val !== null && typeof val !== 'undefined').join('-') + '-' + activity.from.id;

            if (ChannelId == null)
            {
                throw new Exception("missing activity.channelId");
            }

            if (ConversationId == null)
            {
                throw new Exception("missing activity.conversation.id");
            }

            return $"{ ChannelId }/ conversations /{ ConversationId }/{ this.namespace }";
        }
    }
}
