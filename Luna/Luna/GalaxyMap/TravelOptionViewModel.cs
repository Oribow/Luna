using Luna.Biz.DataTransferObjects;
using Luna.Biz.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.GalaxyMap
{
    class TravelOptionViewModel : BaseViewModel
    {
        public ICommand TravelHere { get; }
        public string LocationName { get; }

        private Guid sceneId;

        public TravelOptionViewModel(SceneDataInfoDTO location, Action<Guid> travelHereCallback)
        {
            LocationName = location.Name;
            sceneId = location.Id;
            TravelHere = new Command(() => travelHereCallback(sceneId));
        }

        
    }
}
