using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luna.Views
{
    public class OverflowLabel : Label
    {
        // Passed up by renderer. Best way to do this sadly.
        public Func<int, int> GetLineEnd { get; set; }
        public event Action OnLayoutChangeEvent;

        public void FireLayoutChangeEvent()
        {
            OnLayoutChangeEvent?.Invoke();
        }
    }
}
