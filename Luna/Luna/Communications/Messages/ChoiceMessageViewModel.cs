using Luna.Biz.QuestPlayer;
using Luna.Biz.QuestPlayer.Instructions;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.Communications.Messages
{
    class ChoiceMessageViewModel : Instruction
    {
        public bool ChoiceOpen
        {
            get => choiceOpen;
            set => SetProperty(ref choiceOpen, value);
        }
        public DialogueOptionDTO[] Choices { get; }
        public int SelectedChoiceId { get; private set; }
        public TextMessageViewModel TextMessage
        {
            get => textMessage;
            set => SetProperty(ref textMessage, value);
        }
        public ICommand OnChoiceMade { get; }

        bool choiceOpen = true;
        Action<int> chooseCallback;
        TextMessageViewModel textMessage;

        public ChoiceMessageViewModel(Action<bool> messageCompletedCallback, DialogueOptionDTO[] choices, Action<int> chooseCallback) : base(messageCompletedCallback, true)
        {
            this.Choices = choices;
            this.chooseCallback = chooseCallback;
            OnChoiceMade = new Command<int>(HandleChoiceMade);
        }

        public override void OnStart()
        {

        }

        private void HandleChoiceMade(int index)
        {
            if (!choiceOpen)
                return;

            ChoiceOpen = false;
            SelectedChoiceId = Choices[index].Id;
            TextMessage = new TextMessageViewModel("> " + Choices[index].Name, Color.White, null, false);
            chooseCallback?.Invoke(index);
            OnComplete();
        }
    }
}
