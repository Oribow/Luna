using Luna.Biz.Services;
using Luna.FarCaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.Observation
{
    class ObservationViewModel : BaseViewModel
    {
        public string BackgroundImage
        {
            get => backgroundImage;
            set => SetProperty(ref backgroundImage, value);
        }
        public string LocationName
        {
            get => locationName;
            set => SetProperty(ref locationName, value);
        }
        public string Description { get; }
        public bool IsQuestListOpen { get => isQuestListOpen; set => SetProperty(ref isQuestListOpen, value); }
        public bool IsJumpEnabled { get => isJumpEnabled; set => SetProperty(ref isJumpEnabled, value); }
        public ICommand StartTravelling { get; }
        public ICommand ToogleQuestListVisibility { get; }
        public ICommand CloseAllWindows { get; }
        public List<QuestListItemViewModel> Quests
        {
            get => quests;
            set => SetProperty(ref quests, value);
        }

        string backgroundImage;
        string locationName;
        bool isQuestListOpen;
        SceneService lss;
        QuestService qs;
        bool isJumpEnabled = true;
        List<QuestListItemViewModel> quests;

        public ObservationViewModel(SceneService lss, QuestService qs)
        {
            this.lss = lss;
            this.qs = qs;
            ToogleQuestListVisibility = new Command(() => IsQuestListOpen = !IsQuestListOpen);
            StartTravelling = new Command(HandleJump);
            CloseAllWindows = new Command(() => IsQuestListOpen = false);

            LoadData();
        }

        private async void LoadData()
        {
            var scene = await lss.GetCurrentScene(App.PlayerId);
            var quests = await qs.ListAvailableQuests(App.PlayerId);
            Quests = quests.Select(q => new QuestListItemViewModel(q.Name, q.SceneId, q.Id)).ToList();

            LocationName = scene.Name;
            BackgroundImage = await scene.BackgroundImage;
        }

        private async void HandleJump()
        {
            try
            {
                IsJumpEnabled = false;
                await lss.Travel(App.PlayerId);
                await Application.Current.MainPage.Navigation.PushAsync(new FarCasterPage());
            }
            catch (InvalidOperationException e)
            {
                // TODO: do something here later
            }
        }
    }
}
