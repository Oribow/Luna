using Luna.Biz.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace Luna.Biz.QuestPlayer.Messages
{
    public abstract class Message
    {
        [YamlIgnore]
        public virtual bool MarksStreamEnd => false;
        [YamlIgnore]
        public virtual bool RequiresSaveOnComplete => false;
    }
}
