using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luna.Communications.Messages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChoiceMessageView : ContentView
    {
        public ChoiceMessageView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext == null)
                return;

            ButtonLayout.Children.Clear();

            var optionVM = (ChoiceMessageViewModel)BindingContext;
            for (int i = 0; i < optionVM.Choices.Length; i++)
            {
                var btn = new Button()
                {
                    Text = optionVM.Choices[i].Name,
                    CommandParameter = i,
                    Command = optionVM.OnChoiceMade,
                    TextTransform = TextTransform.None,
                };
                ButtonLayout.Children.Add(btn);
            }
        }
    }
}