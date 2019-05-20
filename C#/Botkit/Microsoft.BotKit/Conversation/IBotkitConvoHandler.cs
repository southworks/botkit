// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Microsoft.BotKit.Conversation
{
    /// <summary>
    /// Definition of the handler functions used to handle .ask and .addQuestion conditions
    /// </summary>
    public interface IBotkitConvoHandler
    {
        Task ConvoHandler(string answer, BotkitDialogWrapper convo, BotWorker bot);
    }
}
