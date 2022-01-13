using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.Models
{
    public enum GameState
    {
        Traveling,
        Observing,
        Dead
    }

    public class Player
    {
        public int Id { get; set; }
        public Guid? CurrentSceneId { get; internal set; }
        public DateTime StateTransitionTimeUTC { get; internal set; }
        public DateTime StateStartTimeUTC { get; internal set; }
        public List<VisitedScene> VisitedScenes { get; internal set; }
        public GameState GameState { get; internal set; }

        private Player() { }

        public Player(int id)
        {
            this.Id = id;
            StateStartTimeUTC = DateTime.UtcNow;
            StateTransitionTimeUTC = DateTime.UtcNow + new TimeSpan(0, 0, 30);
        }
    }
}
