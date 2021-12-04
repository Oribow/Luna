﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luna.Communications.Messages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageMessageView : ContentView
    {
        public ImageMessageView()
        {
            InitializeComponent();
            image.FadeTo(1, 1000).ContinueWith((canceled) =>
            {
                if (BindingContext != null)
                    MainThread.BeginInvokeOnMainThread(
                    ((ImageMessageViewModel)BindingContext).OnComplete);
            });
        }
    }
}