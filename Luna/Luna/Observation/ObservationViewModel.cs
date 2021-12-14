using Luna.Biz.DataTransferObjects;
using Luna.Biz.Services;
using Luna.Communications;
using Luna.Extensions;
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
        public string BackgroundImage => scene.BackgroundImage;
        public string LocationName => scene.Name;
        public string Description { get; }
        public bool IsJumpBtnEnabled { get => isJumpBtnEnabled; set => SetProperty(ref isJumpBtnEnabled, value); }
        public bool IsQuestBtnEnabled { get => isQuestBtnEnabled; set => SetProperty(ref isQuestBtnEnabled, value); }
        public ICommand StartTravelling { get; }
        public ICommand OpenQuestLog { get; }

        SceneDTO scene;
        SceneService lss;
        QuestLogService qs;
        bool isJumpBtnEnabled = true;
        bool isQuestBtnEnabled = true;

        public ObservationViewModel(SceneService lss, QuestLogService qs)
        {
            this.lss = lss;
            this.qs = qs;
            StartTravelling = new Command(HandleJump);
            OpenQuestLog = new Command(HandleOpenQuestLog);

            LoadData();
        }

        private async void LoadData()
        {
            scene = await lss.GetCurrentScene(App.PlayerId);
            OnPropertyChanged(nameof(LocationName));
            OnPropertyChanged(nameof(BackgroundImage));
        }

        private async void HandleJump()
        {
            try
            {
                IsJumpBtnEnabled = false;
                await lss.Travel(App.PlayerId);
                await Application.Current.MainPage.Navigation.SwapPage(new FarCasterPage());
                IsJumpBtnEnabled = true;
            }
            catch (InvalidOperationException e)
            {
                // TODO: do something here later
            }
        }

        private async void HandleOpenQuestLog() {
            IsQuestBtnEnabled = false;
            await Application.Current.MainPage.Navigation.PushAsync(new QuestLogPage(scene.Id));
            IsQuestBtnEnabled = true;
        }
    }
}
