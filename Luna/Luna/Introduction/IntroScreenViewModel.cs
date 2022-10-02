using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Introduction
{
    class IntroScreenViewModel
    {
        public string Headline { get; }
        public string Text { get; }
        public string Image { get; }
        public bool ShowCompleteButton { get; }

        public IntroScreenViewModel(string headline, string text, string image, bool showCompleteButton)
        {
            Headline = headline;
            Text = text;
            Image = image;
            ShowCompleteButton = showCompleteButton;
        }
    }
}
