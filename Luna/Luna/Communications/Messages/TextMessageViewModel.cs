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

        public TextMessageViewModel(TextMessage message) : base(message)
        {
            typingAnimationWanted = !IsCompleted;
            CompleteMessage = new Command((param) => {
                if (((string)param) == null)
                    return;

                Complete(false);
                TypingAnimationWanted = false;
            });
            Skip = new Command(() => TypingAnimationWanted = false);
        }
    }
}
