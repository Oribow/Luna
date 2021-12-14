using Luna.Biz.QuestPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Communications.Messages
{
    abstract class BaseMessage<T> : BaseViewModel, IMessageViewModel where T : Message
    {
        public event Action<Message, bool> OnComplete;
        public bool IsCompleted { get; private set; }

        protected readonly bool isNew;
        protected readonly T message;

        protected BaseMessage(bool isNew, T message)
        {
            this.isNew = isNew;
            this.message = message;
        }

        public virtual void OnStart()
        {

        }

        protected void Complete(bool autoContinue)
        {
            IsCompleted = true;
            OnComplete?.Invoke(message, autoContinue);
        }
    }
}
