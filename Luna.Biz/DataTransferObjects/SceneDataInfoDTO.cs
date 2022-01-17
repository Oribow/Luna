using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.DataTransferObjects
{
    public class SceneDataInfoDTO
    {
        public Guid Id { get; }
        public string Name { get; }
        public string BackgroundImage { get; }

        public SceneDataInfoDTO(Guid id, string name, string backgroundImage)
        {
            Id = id;
            Name = name;
            BackgroundImage = backgroundImage;
        }
    }
}
