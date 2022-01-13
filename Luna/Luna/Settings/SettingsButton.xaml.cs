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
    public partial class SettingsButton : ContentView
    {
        bool buttonWasPressed;

        public SettingsButton()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (buttonWasPressed)
                return;

            buttonWasPressed = true;
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
            buttonWasPressed = false;
        }
    }
}