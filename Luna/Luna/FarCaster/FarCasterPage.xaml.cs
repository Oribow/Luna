using Autofac;
using Luna.Biz.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luna.FarCaster
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FarCasterPage : ContentPage
    {
        public FarCasterPage()
        {
            InitializeComponent();
            var gss = App.Container.Resolve<IGameStateService>();
            BindingContext = new FarCasterViewModel(gss);
        }
    }
}