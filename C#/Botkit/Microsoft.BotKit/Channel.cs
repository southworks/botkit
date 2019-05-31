// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.BotKit
{
    public class Channel
    {
        public Channel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id {get;set;}
        public string Name {get;set;}
    }
}
