using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luna.GalaxyMap
{
    abstract class ContextWindowViewModel : BaseViewModel
    {
        public abstract string MainButtonText { get; }
        public ICommand MainButtonPressed { get; }
        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }

        bool isVisible = true;
        protected readonly int sceneId;

        public ContextWindowViewModel(int sceneId)
        {
            MainButtonPressed = new Command(HandleMainButtonPressed);
            this.sceneId = sceneId;
        }

        protected virtual void HandleMainButtonPressed()
        {
            IsVisible = false;
        }
    }
}
