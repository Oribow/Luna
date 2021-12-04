using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.QuestPlayer.Instructions
{
    public class ChoiceMessageDTO : InstructionDTO
    {
        public DialogueOptionDTO[] Choices { get; set; }
    }
}
