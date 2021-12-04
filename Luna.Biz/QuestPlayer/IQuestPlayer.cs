using Luna.Biz.QuestPlayer.Instructions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.QuestPlayer
{
    public interface IQuestPlayer
    {
        InstructionDTO NextInstruction();
        void SelectOption(int option);
    }
}
