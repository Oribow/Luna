using Luna.Biz.DataTransferObjects;
using Luna.Biz.Services;
using Luna.Biz.ShipAISystems;
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
        IGameStateService gameStateService;
        
        bool isJumpBtnEnabled = true;
        bool isQuestBtnEnabled = true;

        public ObservationViewModel(IGameStateService gss)
        {
            this.gameStateService = gss;
            StartTravelling = new Command(HandleJump);
            OpenQuestLog = new Command(HandleOpenQuestLog);

            LoadData();
        }

        private async void LoadData()
        {
            scene = await gameStateService.GetPlayerScene(App.PlayerId);
            
            OnPropertyChanged(nameof(LocationName));
            OnPropertyChanged(nameof(BackgroundImage));
        }

        private async void HandleJump()
        {
            try
            {
                IsJumpBtnEnabled = false;
                await gameStateService.StartTravelling(App.PlayerId);
                await Application.Current.MainPage.Navigation.ClearAndSetPage(new FarCasterPage(true));
                IsJumpBtnEnabled = true;
            }
            catch (InvalidOperationException e)
            {
                // TODO: do something here later
            }
        }

        private async void HandleOpenQuestLog()
        {
            IsQuestBtnEnabled = false;
            await Application.Current.MainPage.Navigation.PushAsync(new QuestLogPage(scene.Id));
            IsQuestBtnEnabled = true;
        }
    }
}
