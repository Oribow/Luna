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

        public bool HasReachedEnd
        {
            get => hasReachedEnd;
            set
            {
                SetProperty(ref hasReachedEnd, value);
                OnContinue?.ChangeCanExecute();
                OnPropertyChanged(nameof(OnContinueEnabled));
            }
        }

        public Command OnContinue
        {
            get => onContinue;
            set
            {
                SetProperty(ref onContinue, value);
                OnPropertyChanged(nameof(OnContinueEnabled));
            }
        }
        public bool OnContinueEnabled { get => onContinue?.CanExecute(null) ?? false; }
        public string BackgroundImage { get => backgroundImage; set => SetProperty(ref backgroundImage, value); }

        private bool ReadyForNextMessage
        {
            get => readyForNextMessage;
            set
            {
                readyForNextMessage = value;
                OnContinue?.ChangeCanExecute();
                OnPropertyChanged(nameof(OnContinueEnabled));
            }
        }

        string backgroundImage;
        bool hasReachedEnd = false;

        Command onContinue;
        bool readyForNextMessage = false;

        Command nextMessage;

        IQuestLogSession questSession;

        readonly QuestLogService questService;
        readonly GameStateService gameStateService;

        public QuestLogViewModel(QuestLogService questService, GameStateService gameStateService)
        {
            this.questService = questService;
            this.gameStateService = gameStateService;
            nextMessage = new Command(Continue, () => ReadyForNextMessage && !HasReachedEnd);

            OnContinue = nextMessage;
        }

        public async Task LoadQuestLog(Guid locationId)
        {
            questSession = await questService.GetOrCreateQuestLogSession(locationId, App.PlayerId);
            var history = (await questSession.GetHistory()).ToArray();

            for (int iMsg = 0; iMsg < history.Length; iMsg++)
                ProcessMessage(history[iMsg], false);

            ReadyForNextMessage = true;
        }

        async void Continue()
        {
            ReadyForNextMessage = false;
            OnContinue = null;

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

            if (message.MarksStreamEnd)
            {
                ReadyForNextMessage = false;
                HasReachedEnd = true;
            }

            OnContinue = nextMessage;
            ReadyForNextMessage = true;
            if (autoContinue)
            {
                Continue();
            }
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
            else if (instrType == typeof(DeathMessage))
            {
                var deathMsg = (DeathMessage)message;
                msgVM = new DeathViewModel(isNew, deathMsg, gameStateService);
            }
            else
            {
                throw new ArgumentException();
            }

            DisplayMessages.Add(msgVM);
            if (isNew)
            {
                msgVM.OnComplete += OnMessageCompleted;
                OnContinue = msgVM.Skip;
            }
            msgVM.OnStart();
        }
    }
}
