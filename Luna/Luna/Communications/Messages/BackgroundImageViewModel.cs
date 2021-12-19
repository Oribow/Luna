using Luna.Biz.QuestPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Communications.Messages
{
    class BackgroundImageViewModel : BaseMessage<BackgroundImageMessage>
    {
        public string ImageSrc => message.ImagePath;

        public BackgroundImageViewModel(BackgroundImageMessage msg) : base(msg)
        {
        }

        public override void OnStart()
        {
            Complete(false);
        }
    }
}
