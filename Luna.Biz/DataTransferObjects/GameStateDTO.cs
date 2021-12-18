﻿using Luna.Biz.Models;
using Luna.Biz.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.DataTransferObjects
{
    public class GameStateDTO
    {
        public GameState State { get; }
        public DateTime StateTransitionTimeUTC { get; }

        public GameStateDTO(GameState gameState, DateTime stateTransitionTimeUTC)
        {
            State = gameState;
            StateTransitionTimeUTC = stateTransitionTimeUTC;
        }
    }
}
