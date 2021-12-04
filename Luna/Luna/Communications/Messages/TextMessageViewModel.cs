using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.Communications.Messages
{
    class TextMessageViewModel : Instruction
    {
        public string Text { get; }
        public Color TextColor { get; }

        public TextMessageViewModel(string text, Color textColor, Action<bool> messageCompletedCallback, bool autoContinue) : base(messageCompletedCallback, autoContinue)
        {
            this.Text = text;
        }

        public override void OnStart()
        {
            OnComplete();
        }
    }
}
