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
        private static void StartCountdown(FarCasterViewModel self)
        {
            var weakView = new WeakReference<FarCasterViewModel>(self);
            Func<bool> updateCallback = () =>
            {
                FarCasterViewModel vm;
                if (!weakView.TryGetTarget(out vm))
                {
                    return false;
                }

                TimeSpan timeLeft = self.TimeOfArrival - DateTime.UtcNow;
                if (timeLeft.Ticks < 0)
                {
                    vm.TimeOfArrivalLabel = "00:00:00";
                    vm.CanArrive = true;
                    return false;
                }
                else
                {
                    vm.TimeOfArrivalLabel = $"{timeLeft.TotalHours:00}:{timeLeft.Minutes:00}:{timeLeft.Seconds:00}";
                    return true;
                }
            };

            Device.StartTimer(new TimeSpan(0, 0, 1), updateCallback);
        }


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
        public DateTime TimeOfArrival { get => timeOfArrivalUTC; }

        string timeLeftToTravel;
        bool canArrive;
        DateTime timeOfArrivalUTC;
        Command arrive;
        bool arriveWasPressed;

        readonly SceneService sceneService;

        public FarCasterViewModel(SceneService locService)
        {
            this.sceneService = locService;
            arrive = new Command(HandleArrive, (state) => !arriveWasPressed);
            LoadData();
        }

        private async void LoadData()
        {
            var travelState = await sceneService.GetTravelState(App.PlayerId);
            this.timeOfArrivalUTC = travelState.TimeOfArrival;
            CanArrive = timeOfArrivalUTC < DateTime.UtcNow;
            StartCountdown(this);
        }

        private async void HandleArrive(object state)
        {
            arriveWasPressed = true;
            arrive.ChangeCanExecute();

            var scene = await sceneService.Arrive(App.PlayerId);
            await Application.Current.MainPage.Navigation.SwapPage(new ObservationPage());
        }
    }
}
