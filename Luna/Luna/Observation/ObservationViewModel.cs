using Luna.Biz.DataTransferObjects;
using Luna.Biz.Services;
using Luna.Biz.ShipAISystems;
using Luna.Communications;
using Luna.Extensions;
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
        public string BackgroundImage => sceneData.BackgroundImage;
        public string LocationName => sceneData.Name;
        public string Description { get; }
        public bool IsQuestBtnEnabled { get => isQuestBtnEnabled; set => SetProperty(ref isQuestBtnEnabled, value); }
        public ICommand OpenQuestLog { get; }

        SceneDataInfoDTO sceneData;
        PlayerService playerService;
        SceneService sceneService;
        
        bool isQuestBtnEnabled = true;

        public ObservationViewModel(PlayerService gss, SceneService sceneService)
        {
            this.playerService = gss;
            this.sceneService = sceneService;
            OpenQuestLog = new Command(HandleOpenQuestLog);

            LoadData();
        }

        private async void LoadData()
        {
            var playerInfo = await playerService.GetPlayersState(App.PlayerId);
            sceneData = await sceneService.GetSceneDataInfo(playerInfo.CurrentSceneId);

            OnPropertyChanged(nameof(LocationName));
            OnPropertyChanged(nameof(BackgroundImage));
        }

        private async void HandleOpenQuestLog()
        {
            IsQuestBtnEnabled = false;
            //await Application.Current.MainPage.Navigation.PushAsync(new QuestLogPage(sceneData));
            IsQuestBtnEnabled = true;
        }
    }
}
