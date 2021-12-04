using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.Models
{
    public class AvailableQuest
    {
        public int Id { get; set; }
        public string QuestId { get; set; }

        public AvailableQuest(string questId)
        {
            QuestId = questId;
        }
    }
}
