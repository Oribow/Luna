using Luna.Biz.Models;
using Luna.Biz.QuestPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Luna.Biz.QuestPlayer
{
    class MessageSerializer
    {
        Dictionary<QuestMessageType, Type> messageTypeToType;
        Dictionary<Type, QuestMessageType> typeToMessageType;

        IDeserializer deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        ISerializer serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        public MessageSerializer() {
            messageTypeToType = new Dictionary<QuestMessageType, Type>()
            {
                { QuestMessageType.Text, typeof(TextMessage) },
                { QuestMessageType.Image, typeof(ImageMessage) },
                { QuestMessageType.BackgroundImageCmd, typeof(BackgroundImageMessage) },
                { QuestMessageType.Choice, typeof(ChoiceMessage) },
                { QuestMessageType.Wait, typeof(WaitMessage) },
                { QuestMessageType.Death, typeof(DeathMessage) },
                { QuestMessageType.EndOfStream, typeof(EndOfStreamMessage) },
            };
            typeToMessageType = messageTypeToType.ToDictionary(x => x.Value, x => x.Key);
        }

        public QuestMessage Serialize(Message msg, int questLogId)
        {
            var value = serializer.Serialize(msg);
            var msgType = typeToMessageType[msg.GetType()];
            return new QuestMessage(questLogId, value, msgType);
        }

        public Message Deserialize(QuestMessage msg)
        {
            Type msgType = messageTypeToType[msg.MessageType];
            return (Message)deserializer.Deserialize(msg.Value, msgType);
        }
    }
}
