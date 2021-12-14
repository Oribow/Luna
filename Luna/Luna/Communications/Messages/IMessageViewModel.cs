using Luna.Biz.QuestPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Communications.Messages
{
    interface IMessageViewModel
    {
        void OnStart();
        event Action<Message, bool> OnComplete;
        bool IsCompleted { get; }

    }
}
