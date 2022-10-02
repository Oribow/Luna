using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Luna.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Luna.Droid.Services
{
    class EmailService : IEmailService
    {
        public void OpenEmailClient(string to, string subject, string body)
        {
            Intent intent = new Intent(Intent.ActionView);
            Android.Net.Uri data = Android.Net.Uri.Parse($"mailto:{to}?subject={subject}&body={body}");
            intent.SetData(data);

            MainActivity.Context.StartActivity(Intent.CreateChooser(intent, "Send Mail"));
        }
    }
}