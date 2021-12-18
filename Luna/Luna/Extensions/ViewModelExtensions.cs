using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luna.Extensions
{
    static class ViewModelExtensions
    {
        public static void StartCountdown<T>(T self, DateTime countdownEnd, Action<T, TimeSpan> labelUpdateAction, Action<T> countdownEndedAction) where T : class
        {
            var weakView = new WeakReference<T>(self);
            Func<bool> updateCallback = () =>
            {
                T vm;
                if (!weakView.TryGetTarget(out vm))
                {
                    return false;
                }

                TimeSpan timeLeft = countdownEnd - DateTime.UtcNow;
                if (timeLeft.Ticks < 0)
                {
                    countdownEndedAction(vm);
                    return false;
                }
                else
                {
                    labelUpdateAction(vm, timeLeft);
                    return true;
                }
            };

            Device.StartTimer(new TimeSpan(0, 0, 1), updateCallback);
        }
    }
}
