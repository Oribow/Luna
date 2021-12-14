using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Luna.Biz.QuestPlayer.Messages
{
    public class TextMessage : Message
    {
        public string Text { get; set; }

        public TextMessage(string text)
        {
            Text = text;
        }

        public TextMessage()
        {
        }
    }
}
