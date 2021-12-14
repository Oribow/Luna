using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luna.Communications.Messages
{
    class MessageTemplateSelector : DataTemplateSelector
    {
        // MUST BE >= 20 different things because of android
        public DataTemplate TextMessageTemplate { get; set; }
        public DataTemplate ImageMessageTemplate { get; set; }
        public DataTemplate ChoiceMessageTemplate { get; set; }
        public DataTemplate BackgroundImageTemplate { get; set; }
        public DataTemplate WaitTemplate { get; set; }
        public DataTemplate EndOfStreamTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemType = item.GetType();

            if (itemType == typeof(ImageMessageViewModel))
                return ImageMessageTemplate;
            if (itemType == typeof(ChoiceMessageViewModel))
                return ChoiceMessageTemplate;
            if (itemType == typeof(TextMessageViewModel))
                return TextMessageTemplate;
            if (itemType == typeof(BackgroundImageViewModel))
                return BackgroundImageTemplate;
            if (itemType == typeof(WaitViewModel))
                return WaitTemplate;
            if (itemType == typeof(EndOfStreamViewModel))
                return EndOfStreamTemplate;

            throw new ArgumentException("Unknown feed item type of " + item.GetType());
        }
    }
}
