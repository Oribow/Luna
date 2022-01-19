using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.DataTransferObjects
{
    public class SceneDataInfoDTO
    {
        public Guid Id { get; }
        public string Name { get; }
        public string BackgroundImage { get; }
        public Vector2 Position { get; }

        public SceneDataInfoDTO(Guid id, string name, string backgroundImage, Vector2 position)
        {
            Id = id;
            Name = name;
            BackgroundImage = backgroundImage;
            Position = position;
        }
    }
}
