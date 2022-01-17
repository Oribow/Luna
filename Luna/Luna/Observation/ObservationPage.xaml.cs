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

            var ps = App.Container.Resolve<PlayerService>();
            var ss = App.Container.Resolve<SceneService>();
            BindingContext = new ObservationViewModel(ps, ss);
        }
    }
}