using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.QuestPlayer.Messages
{
    public class WaitMessage : Message
    {
        public WaitMessage()
        {
        }

        public WaitMessage(DateTime waitTillUtc)
        {
            //WaitTillUTC = DateTime.UtcNow + new TimeSpan(0, 0, 10);
            this.WaitTillUTC = waitTillUtc;
        }


        public DateTime WaitTillUTC { get; set; }
    }
}
