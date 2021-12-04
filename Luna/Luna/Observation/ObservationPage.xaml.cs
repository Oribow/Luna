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

            var lss = App.Container.Resolve<SceneService>();
            var qs = App.Container.Resolve<QuestService>();
            BindingContext = new ObservationViewModel(lss, qs);
        }
    }
}