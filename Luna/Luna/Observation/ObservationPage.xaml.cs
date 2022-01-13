using Autofac;
using Luna.Biz.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luna.Observation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ObservationPage : ContentPage
    {
        public ObservationPage(bool isNewArrival)
        {
            InitializeComponent();

            var gss = App.Container.Resolve<IGameStateService>();
            BindingContext = new ObservationViewModel(gss);
        }

        private async void JumpButton_Clicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirm jump", "Do you really want to leave? You won't be able to come back.", "Leave", "Stay");

            if (confirm)
                ((ObservationViewModel)BindingContext).StartTravelling.Execute(null);
        }
    }
}