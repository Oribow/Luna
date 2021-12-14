using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.QuestPlayer.Messages
{
    public class ImageMessage : Message
    {
        public string ImagePath { get; set; }

        public ImageMessage(string imagePath)
        {
            ImagePath = imagePath;
        }

        public ImageMessage() { }
    }
}
