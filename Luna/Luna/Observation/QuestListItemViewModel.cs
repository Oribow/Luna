using Luna.Communications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.Observation
{
    class QuestListItemViewModel
    {
        public string Name { get; }
        public ICommand StartQuest { get; }


        public QuestListItemViewModel(string name, Guid locId, string questId)
        {
            Name = name.ToUpper();
            StartQuest = new Command(() => Application.Current.MainPage.Navigation.PushAsync(new QuestPlayerPage(locId, questId)));
        }
    }
}
