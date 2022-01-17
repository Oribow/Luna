using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace Luna.Biz.Models
{
    internal class AssignedScene
    {
        public int Id { get; set; }
        public Guid SceneDataId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        [NotMapped]
        public Vector2 Position => new Vector2(PositionX, PositionY);
        public string LocationName { get; set; }
        public bool HasBeenVisited { get; set; }

        // belongs to
        public Player Player { get; set; }
        public int PlayerId { get; set; }

        private AssignedScene() { }

        public AssignedScene(Guid sceneDataId, float positionX, float positionY, int playerId, string locationName)
        {
            SceneDataId = sceneDataId;
            PositionX = positionX;
            PositionY = positionY;
            PlayerId = playerId;
            LocationName = locationName;
        }
    }
}
