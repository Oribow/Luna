using Luna.Biz.QuestPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Communications.Messages
{
    class EndOfStreamViewModel : BaseMessage<EndOfStreamMessage>
    {
        public EndOfStreamViewModel(EndOfStreamMessage message) : base(message)
        {
        }

        public override void OnStart()
        {
            Complete(false);
        }
    }
}
