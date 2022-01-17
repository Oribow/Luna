using Luna.Biz.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.GalaxyMap
{
    class JumpContextWindow : ContextWindowViewModel
    {
        public override string MainButtonText => "Jump";

        readonly PlayerService playerService;

        public JumpContextWindow(int sceneId, PlayerService playerService) : base(sceneId)
        {
            this.playerService = playerService;
        }

        protected override async void HandleMainButtonPressed()
        {
            base.HandleMainButtonPressed();
            await playerService.LetPlayerTravelTo(App.PlayerId, sceneId);
            MessagingCenter.Send(playerService, "player_started_traveling");
        }
    }
}
