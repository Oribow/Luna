using Luna.Biz.QuestPlayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.DataTransferObjects
{
    public class PlayableQuestDTO : QuestDTO
    {
        public IQuestPlayer QuestPlayer { get; }

        public PlayableQuestDTO(Guid sceneId, string id, string name, IQuestPlayer questPlayer) : base(sceneId, id, name)
        {
            QuestPlayer = questPlayer;
        }
    }
}
