using Autofac;
using Luna.Biz.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luna.ShipAISystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShipAIView : ContentView
    {
        public ShipAIView()
        {
            InitializeComponent();

            LoadData();
        }

        async void LoadData()
        {
            ShipAIService sais = App.Container.Resolve<ShipAIService>();
            var shipAi = await sais.GetShipAI(App.PlayerId);
            var vm = new ShipAIViewModel(shipAi);
            BindingContext = vm;

            await Task.Delay(1000 * 3);
            vm.OnAppears();
        }
    }
}