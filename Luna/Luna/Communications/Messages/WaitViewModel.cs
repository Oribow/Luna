using Luna.Biz.QuestPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luna.Communications.Messages
{
    class WaitViewModel : BaseMessage<WaitMessage>
    {
        private static void StartCountdown(WaitViewModel self)
        {
            self.TimeToWaitLabel = "Time to wait -";
            var weakView = new WeakReference<WaitViewModel>(self);
            Func<bool> updateCallback = () =>
            {
                WaitViewModel vm;
                if (!weakView.TryGetTarget(out vm))
                {
                    return false;
                }

                TimeSpan timeLeft = self.TimeToWait - DateTime.UtcNow;
                if (timeLeft.Ticks < 0)
                {
                    vm.TimeToWaitLabel = "Ready!";
                    vm.Complete(false);
                    return false;
                }
                else
                {
                    vm.TimeToWaitLabel = $"Time to wait - {Math.Floor(timeLeft.TotalHours):00}:{timeLeft.Minutes:00}:{timeLeft.Seconds:00}";
                    return true;
                }
            };

            Device.StartTimer(new TimeSpan(0, 0, 1), updateCallback);
        }


        public string TimeToWaitLabel { get => timeToWaitLabel; set => SetProperty(ref timeToWaitLabel, value); }
        public DateTime TimeToWait => message.WaitTillUTC;

        string timeToWaitLabel;
        public WaitViewModel(bool isNew, WaitMessage message) : base(isNew, message)
        {
        }

        public override void OnStart()
        {
            StartCountdown(this);
        }
    }
}
