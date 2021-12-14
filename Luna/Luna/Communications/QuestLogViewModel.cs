using Luna;
using Luna.Biz.DataTransferObjects;
using Luna.Biz.QuestPlayer;
using Luna.Biz.QuestPlayer.Messages;
using Luna.Biz.Services;
using Luna.Communications.Messages;
using Luna.Extensions;
using Luna.Observation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.Communications
{
    class QuestLogViewModel : BaseViewModel
    {
        public ObservableCollection<IMessageViewModel> DisplayMessages { get; } = new ObservableCollection<IMessageViewModel>();
        public bool CanContinue
        {
            get => canContinue;
            set
            {
                SetProperty(ref canContinue, value);
            }
        }

        public bool HasReachedEnd { get => hasReachedEnd; set => SetProperty(ref hasReachedEnd, value); }

        public ICommand OnContinue => onContinue;
        public string BackgroundImage { get => backgroundImage; set => SetProperty(ref backgroundImage, value); }

        string backgroundImage;
        bool canContinue = false;
        bool hasReachedEnd = false;
        IQuestLogSession questSession;
        Command onContinue;

        readonly QuestLogService questService;

        public QuestLogViewModel(QuestLogService questService)
        {
            this.questService = questService;
            onContinue = new Command(Continue);
        }

        public async Task LoadQuestLog(Guid locationId)
        {
            questSession = await questService.GetOrCreateQuestLogSession(locationId, App.PlayerId);
            var history = (await questSession.GetHistory()).ToArray();

            for (int iMsg = 0; iMsg < history.Length - 1; iMsg++)
                ProcessMessage(history[iMsg], false);

            CanContinue = false;
            if (history.Length > 0)
            {
                ProcessMessage(history[history.Length - 1], true);
            }
            else
            {
                CanContinue = true;
                Continue();
            }
        }

        async void Continue()
        {
            if (!CanContinue)
                return;

            CanContinue = false;
            var msg = await questSession.Continue();
            ProcessMessage(msg, true);
        }

        void OnMessageCompleted(Message message, bool autoContinue)
        {
            if (message.GetType() == typeof(ChoiceMessage))
            {
                questSession.SelectOption(((ChoiceMessage)message).SelectedChoice);
            }

            questSession.SaveCompletedMessage(message);
            CanContinue = true;
            if (autoContinue)
                Continue();
        }

        private void ProcessMessage(Message message, bool isNew)
        {
            var instrType = message.GetType();
            IMessageViewModel msgVM;
            if (instrType == typeof(TextMessage))
            {
                var textMessage = (TextMessage)message;
                msgVM = new TextMessageViewModel(isNew, textMessage);
            }
            else if (instrType == typeof(ImageMessage))
            {
                var imageMessage = (ImageMessage)message;
                msgVM = new ImageMessageViewModel(isNew, imageMessage);
            }
            else if (instrType == typeof(ChoiceMessage))
            {
                var choiceMessage = (ChoiceMessage)message;
                msgVM = new ChoiceMessageViewModel(isNew, choiceMessage);
            }
            else if (instrType == typeof(BackgroundImageMessage))
            {
                var bgInstr = (BackgroundImageMessage)message;
                BackgroundImage = bgInstr.ImagePath;
                msgVM = new BackgroundImageViewModel(isNew, bgInstr);
            }
            else if (instrType == typeof(EndOfStreamMessage))
            {
                var eosMsg = (EndOfStreamMessage)message;
                msgVM = new EndOfStreamViewModel(isNew, eosMsg);
            }
            else if (instrType == typeof(WaitMessage))
            {
                var waitMsg = (WaitMessage)message;
                msgVM = new WaitViewModel(isNew, waitMsg);
            }
            else
            {
                throw new ArgumentException();
            }

            DisplayMessages.Add(msgVM);

            if (message.MarksStreamEnd)
            {
                CanContinue = false;
                HasReachedEnd = true;
            }

            if (isNew)
            {
                msgVM.OnComplete += OnMessageCompleted;
            }

            msgVM.OnStart();
            questSession.SaveStartedMessage(message);
        }
    }
}
