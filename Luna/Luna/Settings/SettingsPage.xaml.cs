using Autofac;
using Luna.Biz;
using Luna.Biz.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luna.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            var contextFactory = App.Container.Resolve<IDbContextFactory<LunaContext>>();
            var playerService = App.Container.Resolve<PlayerService>();
            BindingContext = new SettingsViewModel(contextFactory, playerService);
        }

        private async void ResetDataButton_Clicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Delete all data", "Are you sure that you want to reset all data? It CAN NOT be undone.", "Delete Data", "Cancel");

            if (confirm)
            {
                ((SettingsViewModel)BindingContext).ResetData.Execute(null);
            }
        }
    }
}