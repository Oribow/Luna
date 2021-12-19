using Autofac;
using Luna.Biz.Models;
using Luna.Biz.QuestPlayer;
using Luna.Biz.Services;
using System;
using System.Collections.Generic;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luna.Communications
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestLogPage : ContentPage
    {
        public QuestLogPage(Guid locationId)
        {
            InitializeComponent();
            var qs = App.Container.Resolve<QuestLogService>();
            var gss = App.Container.Resolve<IGameStateService>();
            var nm = App.Container.Resolve<INotificationManager>();
            var qpvm = new QuestLogViewModel(qs, gss, nm);
            _ = qpvm.LoadQuestLog(locationId);
            BindingContext = qpvm;
        }
    }
}