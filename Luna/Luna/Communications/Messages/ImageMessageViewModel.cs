﻿using Luna.Biz.QuestPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Luna.Communications.Messages
{
    class ImageMessageViewModel : BaseMessage<ImageMessage>
    {
        public string ImageSrc => message.ImagePath;

        public ImageMessageViewModel(bool isNew, ImageMessage message) : base(isNew, message)
        {
        }

        public override void OnStart()
        {
            Complete(false);
        }
    }
}
