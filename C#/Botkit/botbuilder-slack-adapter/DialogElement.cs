// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace botbuilder_slack_adapter
{
    public class DialogElement
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Dictionary<string, string> OptionList { get; set; }
        public object Options { get; set; }
        public string Type { get; set; }
        public string Subtype { get; set; }
    }
}
