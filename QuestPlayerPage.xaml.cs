using Autofac;
using Luna.Biz.QuestPlayer;
using System.Collections.Generic;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luna.Communications
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestPlayerPage : ContentPage, IQueryAttributable
    {
        public QuestPlayerPage()
        {
            InitializeComponent();
            BindingContext = new QuestPlayerViewModel(App.Container.Resolve<IQuestService>());
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            int questId = int.Parse(HttpUtility.UrlDecode(query["qid"]));

            _ = ((QuestPlayerViewModel)BindingContext).LoadQuest(questId);
        }
    }
}