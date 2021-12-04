using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.Models
{
    public class Player
    {
        public int Id { get; set; }
        public Guid? CurrentSceneId { get; internal set; }
        public DateTime ArrivalPossibleAfterUTC { get; internal set; }
        public List<VisitedScene> VisitedScenes { get; internal set; }

        public bool IsTraveling => CurrentSceneId == null;
        public bool CanArrive => DateTime.UtcNow > ArrivalPossibleAfterUTC;

        private Player() { }

        public Player(int id)
        {
            this.Id = id;
        }
    }
}
