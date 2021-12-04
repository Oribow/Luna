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
    public partial class QuestPlayerPage : ContentPage
    {
        public QuestPlayerPage(Guid locationId, string questName)
        {
            InitializeComponent();
            var qs = App.Container.Resolve<QuestService>();
            var qpvm = new QuestPlayerViewModel(qs);
            _ = qpvm.LoadQuest(locationId, questName);
            BindingContext = qpvm;
        }
    }
}