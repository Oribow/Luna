using Luna.Biz.QuestPlayer;
using Luna.Biz.QuestPlayer.Messages;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.Communications.Messages
{
    class ChoiceMessageViewModel : BaseMessage<ChoiceMessage>
    {
        public bool ChoiceOpen
        {
            get => choiceOpen;
            set => SetProperty(ref choiceOpen, value);
        }
        public DialogueOption[] Choices => message.Choices;
        public string ReplacementText
        {
            get => replacementText;
            set => SetProperty(ref replacementText, value);
        }
        public ICommand OnChoiceMade { get; }

        bool choiceOpen = true;
        string replacementText;

        public ChoiceMessageViewModel(ChoiceMessage msg) : base(msg)
        {
            OnChoiceMade = new Command<int>(HandleChoiceMade);
        }

        public override void OnStart()
        {
            if (message.SelectedChoice != -1)
                HandleChoiceMade(message.SelectedChoice);
        }

        private void HandleChoiceMade(int index)
        {
            if (!choiceOpen)
                return;

            ChoiceOpen = false;
            ReplacementText = "> " + Choices[index].Name;
            message.SelectedChoice = index;
            Complete(true);
        }
    }
}
