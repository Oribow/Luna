using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luna.Communications.Messages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextMessageView : ContentView
    {
        public TextMessageView()
        {
            InitializeComponent();
        }
    }
}