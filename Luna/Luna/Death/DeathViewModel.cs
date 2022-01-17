using Luna.Biz.Services;
using Luna.Extensions;
using Luna.GalaxyMap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.Death
{
    class DeathViewModel : BaseViewModel
    {
        public string ReviveTimerLabel { get => reviveTimerLabel; set => SetProperty(ref reviveTimerLabel, value); }
        public bool CanRevive { get => canRevive; set => SetProperty(ref canRevive, value); }
        public ICommand RevivePlayer { get; }

        readonly PlayerService gameStateService;
        private DateTime reviveTimeUTC;
        private string reviveTimerLabel;
        private bool canRevive = false;

        public DeathViewModel(PlayerService gameStateService)
        {
            this.gameStateService = gameStateService;
            RevivePlayer = new Command(HandleRevivePlayer);
            LoadData();
        }

        async void LoadData()
        {
            var gs = await gameStateService.GetPlayersState(App.PlayerId);
            reviveTimeUTC = gs.LockoutEndUTC;
            ViewModelExtensions.StartCountdown(this,
                reviveTimeUTC,
                (self, timeLeft) => self.ReviveTimerLabel = $"Ponder your death for {timeLeft.TotalHours:00}:{timeLeft.Minutes:00}:{timeLeft.Seconds:00}", (self) => { self.ReviveTimerLabel = "Revive"; self.CanRevive = true; });
        }

        async void HandleRevivePlayer()
        {
            CanRevive = false;
            await gameStateService.RevivePlayer(App.PlayerId);
            await App.Current.MainPage.Navigation.ClearAndSetPage(new GalaxyMapPage());
        }
    }
}
