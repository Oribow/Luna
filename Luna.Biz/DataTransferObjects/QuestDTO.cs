using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.DataTransferObjects
{
    public class QuestDTO
    {
        public Guid SceneId { get; }
        public string Id { get; }
        public string Name { get; }

        public QuestDTO(Guid sceneId, string id, string name)
        {
            SceneId = sceneId;
            Id = id;
            Name = name;
        }
    }
}
