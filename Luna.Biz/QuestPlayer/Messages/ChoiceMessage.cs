using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Luna.Biz.QuestPlayer.Messages
{
    public class ChoiceMessage : Message
    {
        public ChoiceMessage()
        {
        }

        public DialogueOption[] Choices { get; set; }
        public int SelectedChoice { get; set; } = -1;
    }
}
