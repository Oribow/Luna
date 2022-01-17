using Luna.Biz.Models;
using Luna.Biz.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.DataTransferObjects
{
    public class PlayerStateDTO
    {
        public GameState State { get; }
        public int CurrentSceneId { get; }
        public int PrevSceneId { get; }
        public DateTime LockoutEndUTC { get; }
        public DateTime LockoutStartUTC { get; }

        public PlayerStateDTO(GameState state, int currentSceneId, int prevSceneId, DateTime lockoutEndUTC, DateTime lockoutStartUTC)
        {
            State = state;
            CurrentSceneId = currentSceneId;
            PrevSceneId = prevSceneId;
            LockoutEndUTC = lockoutEndUTC;
            LockoutStartUTC = lockoutStartUTC;
        }
    }
}
