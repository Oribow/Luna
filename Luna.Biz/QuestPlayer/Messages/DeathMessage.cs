using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.QuestPlayer.Messages
{
    public class DeathMessage : Message
    {
        public override bool MarksStreamEnd => true;

        public DeathMessage()
        {
        }
    }
}
