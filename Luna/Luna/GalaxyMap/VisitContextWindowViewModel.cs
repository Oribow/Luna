using Luna.Communications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.GalaxyMap
{
    class VisitContextWindowViewModel : ContextWindowViewModel
    { 
        public override string MainButtonText => "Visit";

        public VisitContextWindowViewModel(int sceneId) : base(sceneId)
        {
        }

        protected override void HandleMainButtonPressed()
        {
            base.HandleMainButtonPressed();

            App.Current.MainPage.Navigation.PushAsync(new QuestLogPage(sceneId));
        }
    }
}
