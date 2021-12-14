using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.QuestPlayer.Messages
{
    public class BackgroundImageMessage : Message
    {
        public string ImagePath { get; set; }

        public BackgroundImageMessage()
        {
        }
    }
}
