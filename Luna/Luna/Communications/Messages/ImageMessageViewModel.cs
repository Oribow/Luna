using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Luna.Communications.Messages
{
    class ImageMessageViewModel : Instruction
    {
        public string ImageSrc
        {
            get => imageSrc;
            set => SetProperty(ref imageSrc, value);
        }

        string imageSrc;

        public ImageMessageViewModel(Action<bool> messageCompletedCallback, bool autoContinue, Task<string> imageSrc) : base(messageCompletedCallback, autoContinue)
        {
            imageSrc.ContinueWith((result) => ImageSrc = result.Result);
        }

    }
}
