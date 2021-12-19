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
        readonly IGameStateService gameStateService;
        readonly INotificationManager notificationManager;

        public QuestLogViewModel(QuestLogService questService, IGameStateService gameStateService, INotificationManager notificationManager)
        {
            this.notificationManager = notificationManager;
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
                ProcessMessage(history[iMsg]);

            if (history.Length > 0)
                ReadyForNextMessage = history[history.Length - 1].IsCompleted && !history[history.Length - 1].MarksStreamEnd;
            else
                ReadyForNextMessage = true;
        }

        async void Continue()
        {
            ReadyForNextMessage = false;
            OnContinue = null;

            var msg = await questSession.Continue();
            await questSession.SaveNewMessage(msg);
            ProcessMessage(msg);
        }

        void OnMessageCompleted(Message message, bool autoContinue)
        {
            questSession.SaveExistingMessage(message);
            if (message.GetType() == typeof(ChoiceMessage))
            {
                questSession.SelectOption(((ChoiceMessage)message).SelectedChoice);
            }

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

        private void ProcessMessage(Message message)
        {
            var instrType = message.GetType();
            IMessageViewModel msgVM;
            if (instrType == typeof(TextMessage))
            {
                var textMessage = (TextMessage)message;
                msgVM = new TextMessageViewModel(textMessage);
            }
            else if (instrType == typeof(ImageMessage))
            {
                var imageMessage = (ImageMessage)message;
                msgVM = new ImageMessageViewModel(imageMessage);
            }
            else if (instrType == typeof(ChoiceMessage))
            {
                var choiceMessage = (ChoiceMessage)message;
                msgVM = new ChoiceMessageViewModel(choiceMessage);
            }
            else if (instrType == typeof(BackgroundImageMessage))
            {
                var bgInstr = (BackgroundImageMessage)message;
                BackgroundImage = bgInstr.ImagePath;
                msgVM = new BackgroundImageViewModel(bgInstr);
            }
            else if (instrType == typeof(EndOfStreamMessage))
            {
                var eosMsg = (EndOfStreamMessage)message;
                msgVM = new EndOfStreamViewModel(eosMsg);
            }
            else if (instrType == typeof(WaitMessage))
            {
                var waitMsg = (WaitMessage)message;
                msgVM = new WaitViewModel(waitMsg, notificationManager);
            }
            else if (instrType == typeof(DeathMessage))
            {
                var deathMsg = (DeathMessage)message;
                msgVM = new DeathViewModel(deathMsg, gameStateService);
            }
            else
            {
                throw new ArgumentException();
            }

            DisplayMessages.Add(msgVM);
            if (!message.IsCompleted)
            {
                msgVM.OnComplete += OnMessageCompleted;
                OnContinue = msgVM.Skip;
            }
            msgVM.OnStart();
        }
    }
}
