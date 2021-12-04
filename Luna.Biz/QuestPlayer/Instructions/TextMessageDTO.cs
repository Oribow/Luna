using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Luna.Biz.QuestPlayer.Instructions
{
    public class TextMessageDTO : InstructionDTO
    {
        public string Text { get; set; }
        public Color TextColor { get; set; }
    }
}
