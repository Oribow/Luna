using Autofac;
using Luna.Biz.Services;
using Luna.Biz;
using Luna.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
//using Xamarin.Essentials;
using Xamarin.Forms;
using Luna.Services;

namespace Luna.Settings
{
    class SettingsViewModel
    {
        public Command ResetData { get; }
        public Command ContactUs { get; }

        private bool resetInProgress = false;
        IDbContextFactory<LunaContext> contextFactory;
        PlayerService playerService;
        IEmailService emailService;

        public SettingsViewModel(IDbContextFactory<LunaContext> contextFactory, PlayerService playerService, IEmailService emailService)
        {
            this.contextFactory = contextFactory;
            this.playerService = playerService;
            this.emailService = emailService;

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

        public void HandleContactUs()
        {
            emailService.OpenEmailClient("lyra-app@outlook.com", "Feedback", "Hi StrangeVoid-team,");
        }
    }
}
