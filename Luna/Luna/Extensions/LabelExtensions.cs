using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Luna.Extensions
{
    static class LabelExtensions
    {
        public static Task<bool> CountdownAnimation(this Label self, DateTime timeUTC)
        {
            var weakView = new WeakReference<Label>(self);
            var tcs = new TaskCompletionSource<bool>();

            Action<double> animCallback = (t) =>
            {
                Label label;
                if (weakView.TryGetTarget(out label))
                {
                    TimeSpan timeLeft = timeUTC - DateTime.UtcNow;
                    if (timeLeft.Ticks < 0)
                        label.Text = "00:00:00";
                    else
                        label.Text = $"{timeLeft.TotalHours:00}:{timeLeft.Minutes:00}:{timeLeft.Seconds:00}";
                }
            };

            uint duration = (uint)((timeUTC - DateTime.UtcNow).TotalMilliseconds + 1000);
            new Animation(
                animCallback, 0, 1, Easing.Linear
            ).Commit(self, nameof(CountdownAnimation), 1000, duration, null, (f, aborted) =>
            {
                tcs.SetResult(aborted);
            });

            return tcs.Task;
        }
    }
}
