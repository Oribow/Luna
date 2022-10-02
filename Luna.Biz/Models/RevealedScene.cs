using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace Luna.Biz.Models
{
    class RevealedScene
    {
        [Key]
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public Guid SceneDataId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        [NotMapped]
        public Vector2 Position
        {
            get => new Vector2(PositionX, PositionY);
            set { PositionX = value.X; PositionY = value.Y; }
        }
        public string MapName { get; set; }

        public RevealedScene(int playerId, Guid sceneDataId, Vector2 position, string mapName)
        {
            PlayerId = playerId;
            SceneDataId = sceneDataId;
            Position = position;
            MapName = mapName;
        }

        private RevealedScene()
        {
        }
    }
}
