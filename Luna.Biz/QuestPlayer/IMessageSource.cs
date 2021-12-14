using Luna.Biz.QuestPlayer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.QuestPlayer
{
    public interface IMessageSource
    {
        Task<Message> Continue();
        void SelectOption(int option);
        void DumpTo(Stream stream);
        void LoadFrom(Stream stream);
    }
}
