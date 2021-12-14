using Luna.Biz.QuestPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.Communications.Messages
{
    class TextMessageViewModel : BaseMessage<TextMessage>
    {
        public string Text => message.Text;

        public TextMessageViewModel(bool isNew, TextMessage message) : base(isNew, message)
        {
        }

        public override void OnStart()
        {
            Complete(false);
        }
    }
}
