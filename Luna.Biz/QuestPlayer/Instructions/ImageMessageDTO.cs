using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.QuestPlayer.Instructions
{
    public class ImageMessageDTO : InstructionDTO
    {
        public Task<string> ImagePath { get; set; }
    }
}
