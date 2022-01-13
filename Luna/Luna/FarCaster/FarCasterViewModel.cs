using Luna.Biz.Services;
using Luna.Communications;
using Luna.Extensions;
using Luna.Observation;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.FarCaster
{
    class FarCasterViewModel : BaseViewModel
    {
        public string TimeOfArrivalLabel { get => timeLeftToTravel; set => SetProperty(ref timeLeftToTravel, value); }
        public bool CanArrive
        {
            get => canArrive;
            set
            {
                SetProperty(ref canArrive, value);
            }
        }
        public ICommand Arrive => arrive;
        public string BackgroundImage
        {
            get => backgroundImage;
            set => SetProperty(ref backgroundImage, value);
        }

        string timeLeftToTravel;
        bool canArrive;
        DateTime timeOfArrivalUTC;
        Command arrive;
        bool arriveWasPressed;
        string backgroundImage;

        readonly IGameStateService gameStateService;

        public FarCasterViewModel(IGameStateService gameStateService)
        {
            this.gameStateService = gameStateService;
            arrive = new Command(HandleArrive, (state) => !arriveWasPressed);
            LoadData();
        }

        private async void LoadData()
        {
            var travelState = await gameStateService.GetGameState(App.PlayerId);
            this.timeOfArrivalUTC = travelState.StateTransitionTimeUTC;
            CanArrive = timeOfArrivalUTC < DateTime.UtcNow;
            ViewModelExtensions.StartCountdown(this,
                timeOfArrivalUTC,
                (self, timeLeft) => self.TimeOfArrivalLabel = $"Arrive in {timeLeft.TotalHours:00}:{timeLeft.Minutes:00}:{timeLeft.Seconds:00}",
                (self) => { self.CanArrive = true; self.TimeOfArrivalLabel = "00:00:00"; });

            // slide show didnt look good
            /*
            double percentageComplete;
            if ((travelState.StateTransitionTimeUTC - travelState.StateStartTimeUTC).TotalSeconds == 0)
                percentageComplete = 1;
            else
                percentageComplete = (DateTime.UtcNow - travelState.StateStartTimeUTC).TotalSeconds / (travelState.StateTransitionTimeUTC - travelState.StateStartTimeUTC).TotalSeconds;
            int imageNum = (int)Math.Clamp(Math.Floor(percentageComplete * 7) + 1, 1, 7);
            */
            BackgroundImage = "flower_opening_7";
        }

        private async void HandleArrive(object state)
        {
            arriveWasPressed = true;
            arrive.ChangeCanExecute();

            var scene = await gameStateService.Arrive(App.PlayerId);
            await Application.Current.MainPage.Navigation.ClearAndSetPage(new ObservationPage(true));
        }
    }
}
