using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Communications.Messages
{
    class BackgroundImageViewModel : Instruction
    {
        public string ImageSrc { get; }

        public BackgroundImageViewModel(string imageSrc, Action<bool> messageCompletedCallback) : base(messageCompletedCallback, false)
        {
            ImageSrc = imageSrc;
        }

        public override void OnStart()
        {
            OnComplete();
        }
    }
}
