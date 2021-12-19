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
        public ObservationPage()
        {
            InitializeComponent();

            var gss = App.Container.Resolve<IGameStateService>();
            var qs = App.Container.Resolve<QuestLogService>();
            BindingContext = new ObservationViewModel(gss);
        }
    }
}