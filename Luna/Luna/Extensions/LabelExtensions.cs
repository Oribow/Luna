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

        public static Task<bool> TextTypingAnimation(this Label self, string text, int timePerCharacter = 50)
        {
            if (string.IsNullOrEmpty(text))
            {
                self.Text = text;
                return Task.FromResult(true);
            }

            var weakView = new WeakReference<Label>(self);
            var tcs = new TaskCompletionSource<bool>();

            string textWithPauses = AddPausesToTypeString(text);
            Action<double> animCallback = (t) =>
            {
                Label label;
                if (weakView.TryGetTarget(out label))
                {
                    int charsToShow = (int)(textWithPauses.Length * t);
                    string textToShow = textWithPauses.Substring(0, charsToShow);
                    textToShow = RemovePausesFromTypeString(textToShow);
                    label.Text = textToShow;
                }
            };

            int duration = timePerCharacter * textWithPauses.Length;
            new Animation(
                animCallback, 0, 1, Easing.Linear
            ).Commit(self, nameof(TextTypingAnimation), 16, (uint)duration, null, (f, aborted) =>
            {
                Label label;
                if (aborted && weakView.TryGetTarget(out label))
                    label.Text = text;

                tcs.SetResult(aborted);
            });

            return tcs.Task;
        }

        private static string AddPausesToTypeString(string str)
        {
            return str.Replace(".", ".");
        }

        private static string RemovePausesFromTypeString(string str)
        {
            return str.Replace("", "");
        }
    }
}
