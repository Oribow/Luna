using Autofac;
using Luna.Biz.Services;
using Luna.Biz;
using Luna.Database;
using Microsoft.EntityFrameworkCore;
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
        IDbContextFactory<LunaContext> contextFactory;
        PlayerService playerService;

        public SettingsViewModel(IDbContextFactory<LunaContext> contextFactory, PlayerService playerService)
        {
            this.contextFactory = contextFactory;
            this.playerService = playerService;

            ResetData = new Command(HandleResetData, () => !resetInProgress);
            ContactUs = new Command(HandleContactUs);
        }

        public async void HandleResetData()
        {
            resetInProgress = true;
            ResetData.ChangeCanExecute();

            await GameCreator.StartNewGame(contextFactory, playerService);
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
