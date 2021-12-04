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
        bool choiceMade = false;

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
                btn.Clicked += Btn_Clicked;
                ButtonLayout.Children.Add(btn);
            }
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            if (choiceMade)
                return;
            choiceMade = true;

            ButtonLayout.FadeTo(0, 400).ContinueWith(
                (state) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MainLayout.Children.Remove(ButtonLayout);
                        MainLayout.Children.Add(new TextMessageView()
                        {
                            BindingContext = ((ChoiceMessageViewModel)BindingContext).TextMessage
                        });
                    });
                });
        }
    }
}