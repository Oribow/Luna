using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.Models
{
    public class QuestLog
    {
        public int Id { get; set; }
        public Player Player { get; set; }
        public int PlayerId { get; set; }
        public Guid LocationId { get; set; }

        public string BackgroundImage { get; set; }
        public List<QuestMessage> Messages { get; set; } = new List<QuestMessage>();
        public byte[] DialogState { get; set; }

        private QuestLog() { }

        public QuestLog(int playerId, Guid locationId)
        {
            PlayerId = playerId;
            LocationId = locationId;
        }


    }
}
