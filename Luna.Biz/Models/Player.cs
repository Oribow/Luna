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

        public Guid? CurrentSceneId { get; internal set; }
        public Guid? PrevSceneId { get; internal set; }

        private Player() { }

        public Player(int id)
        {
            this.Id = id;
            this.GameState = GameState.Alive;
        }
    }
}
