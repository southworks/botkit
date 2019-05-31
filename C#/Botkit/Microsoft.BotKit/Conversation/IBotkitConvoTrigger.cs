// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;

namespace Microsoft.BotKit.Conversation
{
    /// <summary>
    /// Definition of the trigger pattern passed into .ask or .addQuestion
    /// </summary>
    public interface IBotkitConvoTrigger
    {
        void BotkitConvoTrigger(IBotkitConvoHandler handler, string type = null, string pattern = null, bool defaultTrigger = false);

        void BotkitConvoTrigger(IBotkitConvoHandler handler, string type = null, Regex pattern = null, bool defaultTrigger = false) ;
    }
}
