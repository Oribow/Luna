using Luna.Biz.QuestPlayer.Messages;
using Luna.Biz.Services;
using Luna.Death;
using Luna.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Communications.Messages
{
    class DeathViewModel : BaseMessage<DeathMessage>
    {
        IGameStateService gss;

        public DeathViewModel(DeathMessage message, IGameStateService gss) : base(message)
        {
            this.gss = gss;
        }

        public override async void OnStart()
        {
            if(!IsCompleted)
            {
                await gss.KillPlayer(App.PlayerId);
                await App.Current.MainPage.Navigation.ClearAndSetPage(new DeathPage());
            }
        }
    }
}
