using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.Models
{
    public class QuestMessage
    {
        public int Id { get; set; }
        public QuestLog QuestLog { get; set; }
        public int QuestLogId { get; set; }

        public string Value { get; set; }
        public QuestMessageType MessageType { get; set; }

        private QuestMessage() { }

        public QuestMessage(int questLogId, string value, QuestMessageType messageType)
        {
            QuestLogId = questLogId;
            Value = value;
            MessageType = messageType;
        }
    }

    public enum QuestMessageType
    {
        Text,
        Image,
        Choice,
        BackgroundImageCmd,
        Wait,
        Death,
        EndOfStream,
    }
}
