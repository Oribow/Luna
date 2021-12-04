using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Luna.Communications.Messages
{
    class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextMessageTemplate { get; set; }
        public DataTemplate ImageMessageTemplate { get; set; }
        public DataTemplate ChoiceMessageTemplate { get; set; }
        public DataTemplate BackgroundImageTemplate { get; set; }

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

            throw new ArgumentException("Unknown feed item type of " + item.GetType());
    }
}
}
