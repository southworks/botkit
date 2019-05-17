// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.BotKit.Conversation
{
    /// <summary>
    /// Definition of the trigger pattern passed into .ask or .addQuestion
    /// </summary>
    public interface IBotkitConvoTrigger
    {
        string Type { get; set; }
        Regex Pattern { get; set; }
        IBotkitConvoHandler Handler { get; set; }
        bool Default { get; set; }
    }
}
