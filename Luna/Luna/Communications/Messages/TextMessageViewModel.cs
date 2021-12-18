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
        public bool TypingAnimationWanted
        {
            get => typingAnimationWanted;
            set => SetProperty(ref typingAnimationWanted, value);
        }
        public ICommand CompleteMessage { get; }

        bool typingAnimationWanted;

        public TextMessageViewModel(bool isNew, TextMessage message) : base(isNew, message)
        {
            typingAnimationWanted = isNew;
            CompleteMessage = new Command(() => { 
                Complete(false);
                TypingAnimationWanted = false;
            });
            Skip = new Command(() => TypingAnimationWanted = false);
        }

        public override void OnStart()
        {
            if (!isNew)
                Complete(false);
        }
    }
}
