using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.Models
{
    internal class QuestLog
    {
        public int Id { get; set; }
        public Player Player { get; set; }
        public int PlayerId { get; set; }
        public Guid SceneDataId { get; set; }

        public string BackgroundImage { get; set; }
        public List<QuestMessage> Messages { get; set; } = new List<QuestMessage>();
        public byte[] DialogState { get; set; }

        private QuestLog() { }

        public QuestLog(int playerId, Guid sceneDataId)
        {
            PlayerId = playerId;
            SceneDataId = sceneDataId;
        }


    }
}
