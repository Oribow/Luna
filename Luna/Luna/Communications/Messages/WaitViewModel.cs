﻿using Luna.Biz.QuestPlayer.Messages;
using Luna.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luna.Communications.Messages
{
    class WaitViewModel : BaseMessage<WaitMessage>
    {
        public string TimeToWaitLabel { get => timeToWaitLabel; set => SetProperty(ref timeToWaitLabel, value); }

        string timeToWaitLabel = "Time to wait -";
        INotificationManager notificationManager;
        public WaitViewModel(WaitMessage message, INotificationManager notificationManager) : base(message)
        {
            this.notificationManager = notificationManager;
        }

        public override void OnStart()
        {
            if (IsCompleted)
            {
                TimeToWaitLabel = "Ready!";
            }
            else
            {
                ViewModelExtensions.StartCountdown(this,
                    message.WaitTillUTC,
                    (self, timeLeft) => self.TimeToWaitLabel = $"Time to wait - {Math.Floor(timeLeft.TotalHours):00}:{timeLeft.Minutes:00}:{timeLeft.Seconds:00}",
                    (self) => { self.TimeToWaitLabel = "Ready!"; self.Complete(false); });
                notificationManager.SendNotification("Continue Story Now!", "You can continue the story now!", message.WaitTillUTC);
            }
        }
    }
}
