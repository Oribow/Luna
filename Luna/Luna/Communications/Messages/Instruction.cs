using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Communications.Messages
{
    abstract class Instruction : BaseViewModel
    {
        private Action<bool> messageCompletedCallback;
        private bool completedCalled = false;
        protected bool autoContinue;

        protected Instruction(Action<bool> messageCompletedCallback, bool autoContinue)
        {
            this.messageCompletedCallback = messageCompletedCallback;
            this.autoContinue = autoContinue;
        }

        public virtual void OnStart()
        {

        }

        public virtual void OnComplete()
        {
            if (completedCalled)
                return;

            messageCompletedCallback?.Invoke(autoContinue);
            completedCalled = true;
        }
    }
}
