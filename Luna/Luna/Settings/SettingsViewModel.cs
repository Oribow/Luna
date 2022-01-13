using Autofac;
using Luna.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Luna.Settings
{
    class SettingsViewModel
    {
        public Command ResetData { get; }
        public Command ContactUs { get; }

        private bool resetInProgress = false;

        public SettingsViewModel()
        {
            ResetData = new Command(HandleResetData, () => !resetInProgress);
            ContactUs = new Command(HandleContactUs);
        }

        public async void HandleResetData()
        {
            resetInProgress = true;
            ResetData.ChangeCanExecute();

            var platformHelper = App.Container.Resolve<IPlatformBootstrapHelper>();
            Bootstrapper bootstrapper = new Bootstrapper(platformHelper);
            await bootstrapper.ResetGameData();
            await ((App)App.Current).OpenLandingPage();
        }

        public async void HandleContactUs()
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "Feedback",
                    Body = "Hi StrangeVoid-team,\n",
                    To = new List<string>() { "lyra-app@outlook.com" },
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }
    }
}
