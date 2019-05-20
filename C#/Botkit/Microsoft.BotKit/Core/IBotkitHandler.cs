// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Microsoft.BotKit.Core
{
    public interface IBotkitHandler
    {
        Task<object> Handler(BotWorker botWorker, IBotkitMessage botkitMessage);
    }
}
