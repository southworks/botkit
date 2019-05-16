// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace botbuilder_slack_adapter
{
    public class DialogData
    {
        public string Title { get; set; }
        public string CallbackId { get; set; }
        public string SubmitLabel { get; set; }
        public List<DialogElement> Elements { get; set; }
        public string State { get; set; }
        public bool NotifyOnCancel { get; set; }

        public DialogData(string title, string callback, string submit, List<DialogElement> elements)
        {
            Title = title;
            CallbackId = callback;
            SubmitLabel = submit;
            Elements = elements;
        }
    }
}
