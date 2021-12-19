using Luna.Biz.QuestPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.Communications.Messages
{
    abstract class BaseMessage<T> : BaseViewModel, IMessageViewModel where T : Message
    {
        public event Action<Message, bool> OnComplete;
        public bool IsCompleted { get; private set; }
        public Command Skip { get; protected set; }

        protected readonly T message;

        protected BaseMessage(T message)
        {
            this.IsCompleted = message.IsCompleted;
            this.message = message;
        }

        public virtual void OnStart()
        {

        }

        protected void Complete(bool autoContinue)
        {
            if (IsCompleted)
                return;

            IsCompleted = true;
            message.IsCompleted = true;
            OnComplete?.Invoke(message, autoContinue);
        }
    }
}
