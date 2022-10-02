using Autofac;
using Luna.Biz.Services;
using Luna.Extensions;
using Luna.GalaxyMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luna.Introduction
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntroductionPage : ContentPage
    {
        public IntroductionPage()
        {
            InitializeComponent();

            Carousel.ItemsSource = new IntroScreenViewModel[]
            {
                new IntroScreenViewModel("Welcome space explorer!",
                @"You are the captain and sole crewmember of a spaceship. This spaceship is special. It features an alien engine technology, which mechanisms have withstood understanding so far.

While engine allows for incredible fast speeds, its controls are utterly incomprehensible. One cannot really steer the engine but only press buttons at random and hope it goes somewhere interesting.", "planet_19", false),
                new IntroScreenViewModel("",
                @"Since no one has yet figured out the button combination for your home world, this is a one-way trip.

And you are ready for it…", "planet_2", false),
                new IntroScreenViewModel("This is a beta",
                "It only contains a handful of stories to test out how well this concept works and if there is any interest in it. We are grateful for any feedback we get. Contact info is in the settings menu.", "planet_7", true)
            };
        }

        bool onlyOnceGuard;
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (onlyOnceGuard)
                return;

            onlyOnceGuard = true;

            var ps = App.Container.Resolve<PlayerService>();
            await ps.PlayerCompletedIntro(App.PlayerId);
            await Navigation.ClearAndSetPage(new GalaxyMapPage());
        }
    }
}