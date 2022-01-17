using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.Models
{
    public enum GameState
    {
        Alive,
        Dead
    }

    internal class Player
    {
        public int Id { get; set; }
        public DateTime LockoutEndUTC { get; internal set; }
        public DateTime LockoutStartUTC { get; internal set; }
        public GameState GameState { get; internal set; }

        public int? CurrentSceneId { get; internal set; }
        public AssignedScene CurrentScene { get; internal set; }
        public int? PrevSceneId { get; internal set; }
        public AssignedScene PrevScene { get; internal set; }

        private Player() { }

        public Player(int id)
        {
            this.Id = id;
            this.GameState = GameState.Alive;
        }
    }
}
