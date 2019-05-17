// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace Microsoft.BotKit
{
    /// <summary>
    ///  A customized version of ConversationState that override the getStorageKey method to create a more complex key value.
    ///  This allows Botkit to automatically track conversation state in scenarios where multiple users are present in a single channel,
    ///  or when threads or sub-channels parent channel that would normally collide based on the information defined in the conversation address field.
    ///  Note: This is used automatically inside Botkit and developers should not need to directly interact with it.
    /// </summary>
    public class BotkitConversationState : ConversationState
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

            return $"{ ChannelId }/ conversations /{ ConversationId }/{ typeof(BotkitConversationState).Namespace }";
        }
    }
}
