using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.DataTransferObjects
{
    public class SceneDTO
    {
        public Guid Id { get; }
        public string IntroQuestId { get; }
        public string Name { get; }
        public Task<string> BackgroundImage { get; }

        public SceneDTO(Guid id, string introQuestId, string name, Task<string> backgroundImage)
        {
            Id = id;
            IntroQuestId = introQuestId;
            Name = name;
            BackgroundImage = backgroundImage;
        }
    }
}
