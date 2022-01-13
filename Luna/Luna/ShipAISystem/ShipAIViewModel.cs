using Luna.Biz.Services;
using Luna.Biz.ShipAISystems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.ShipAISystem
{
    class ShipAIViewModel : BaseViewModel, IShipAIOutput
    {
        // TODO: stop dangling timers, also this whole thing seems unsafe
        static void VanishTimer(ShipAIViewModel self, Action<ShipAIViewModel> timerEnded)
        {
            var weakView = new WeakReference<ShipAIViewModel>(self);
            Func<bool> updateCallback = () =>
            {
                ShipAIViewModel vm;
                if (!weakView.TryGetTarget(out vm))
                {
                    return false;
                }

                TimeSpan timeLeft = vm.HideDialogAfterUTC - DateTime.UtcNow;
                if (timeLeft.Ticks < 1000)
                {
                    timerEnded(vm);
                }
                return true;
            };

            var interval = self.HideDialogAfterUTC - DateTime.UtcNow;
            Device.StartTimer(interval, updateCallback);
        }

        public string DialogText { 
            get => dialogText;
            set => SetProperty(ref dialogText, value);
        }

        public bool ShowDialog
        {
            get => showDialog;
            set => SetProperty(ref showDialog, value);
        }

        public DateTime HideDialogAfterUTC { get; private set; }

        public ICommand Interact { get; }

        string dialogText;
        bool showDialog;

        ShipAI shipAI;

        public ShipAIViewModel(ShipAI shipAI)
        {
            this.shipAI = shipAI;
            Interact = new Command(HandleInteraction);

            shipAI.SetOutput(this);
        }

        public void OnAppears()
        {
            shipAI.BecomesVisible();
        }

        public void Say(string text)
        {
            DialogText = text;
            ShowDialog = true;
            HideDialogAfterUTC = DateTime.UtcNow + new TimeSpan(0, 0, 30);

            VanishTimer(this, (self) => { 
                self.ShowDialog = false;
            });
        }

        void HandleInteraction()
        {
            shipAI.Interact();
        }
    }
}
