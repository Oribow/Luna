using Luna.Biz.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.Services
{
    public interface IGameStateService
    {
        public Task<GameStateDTO> GetGameState(int playerId);

        public Task<SceneDTO> GetPlayerScene(int playerId);

        public Task<SceneDTO> Arrive(int playerId);

        public Task<GameStateDTO> StartTravelling(int playerId);

        public Task RevivePlayer(int playerId);

        public Task<GameStateDTO> KillPlayer(int playerId);
    }
}
