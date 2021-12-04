using Luna;
using Luna.Biz.DataTransferObjects;
using Luna.Biz.QuestPlayer;
using Luna.Biz.QuestPlayer.Instructions;
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
    class QuestPlayerViewModel : BaseViewModel
    {
        public ObservableCollection<Instruction> DisplayMessages { get; } = new ObservableCollection<Instruction>();
        public bool CanContinue
        {
            get => canContinue;
            set
            {
                SetProperty(ref canContinue, value);
                onContinue.ChangeCanExecute();
            }
        }
        public ICommand OnContinue => onContinue;
        public string Title => quest?.Name;
        public string BackgroundImage { get => backgroundImage; set => SetProperty(ref backgroundImage, value); }

        string backgroundImage;
        bool canContinue = false;
        PlayableQuestDTO quest;
        Command onContinue;

        readonly QuestService questService;

        public QuestPlayerViewModel(QuestService questService)
        {
            this.questService = questService;
            onContinue = new Command(LoadNextMessage, () => CanContinue);
        }

        public async Task LoadQuest(Guid locationId, string questName)
        {
            quest = await questService.StartQuest(locationId, questName, App.PlayerId);
            LoadNextMessage();
        }

        void LoadNextMessage()
        {
            var instr = quest.QuestPlayer.NextInstruction();
            if (instr != null)
            {
                CanContinue = false;
                ProcessInstruction(instr);
            }
            else
            {
                CanContinue = false;
                questService.CompleteQuest(quest.SceneId, quest.Id, App.PlayerId).Wait();
                _ = Application.Current.MainPage.Navigation.SwapPage(new ObservationPage());
            }
        }

        void OnChoiceMade(int index)
        {
            quest.QuestPlayer.SelectOption(index);
        }

        void OnMessageCompleted(bool autoContinue)
        {
            CanContinue = true;
            if (autoContinue)
                LoadNextMessage();
        }

        void ProcessInstruction(InstructionDTO instruction)
        {
            var instrType = instruction.GetType();


            Instruction msgVM;
            if (instrType == typeof(TextMessageDTO))
            {
                var textMessage = (TextMessageDTO)instruction;
                msgVM = new TextMessageViewModel(textMessage.Text, textMessage.TextColor, OnMessageCompleted, textMessage.AutoContinue);
            }
            else if (instrType == typeof(ImageMessageDTO))
            {
                var imageMessage = (ImageMessageDTO)instruction;
                msgVM = new ImageMessageViewModel(OnMessageCompleted, imageMessage.AutoContinue, imageMessage.ImagePath);
            }
            else if (instrType == typeof(ChoiceMessageDTO))
            {
                var choiceMessage = (ChoiceMessageDTO)instruction;
                msgVM = new ChoiceMessageViewModel(OnMessageCompleted, choiceMessage.Choices, OnChoiceMade);
            }
            else if (instrType == typeof(BackgroundImageInstructionDTO))
            {
                var bgInstr = (BackgroundImageInstructionDTO)instruction;
                BackgroundImage = bgInstr.ImagePath;
                msgVM = new BackgroundImageViewModel(bgInstr.ImagePath, OnMessageCompleted);
            }
            else
            {
                throw new ArgumentException();
            }

            DisplayMessages.Add(msgVM);
            msgVM.OnStart();
        }
    }
}
