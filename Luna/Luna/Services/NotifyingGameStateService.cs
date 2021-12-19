using Luna.Biz.DataTransferObjects;
using Luna.Biz.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Services
{
    class NotifyingGameStateService : IGameStateService
    {
        readonly GameStateService gameStateService;
        readonly INotificationManager notificationManager;

        public NotifyingGameStateService(GameStateService gameStateService, INotificationManager notificationManager)
        {
            this.gameStateService = gameStateService;
            this.notificationManager = notificationManager;
        }

        public Task<SceneDTO> Arrive(int playerId)
        {
            return gameStateService.Arrive(playerId);
        }

        public Task<GameStateDTO> GetGameState(int playerId)
        {
            return gameStateService.GetGameState(playerId);
        }

        public Task<SceneDTO> GetPlayerScene(int playerId)
        {
            return gameStateService.GetPlayerScene(playerId);
        }

        public async Task<GameStateDTO> KillPlayer(int playerId)
        {
            var state = await gameStateService.KillPlayer(playerId);

            notificationManager.SendNotification("Revive now!", "You can revive now!", state.StateTransitionTimeUTC);
            return state;
        }

        public Task RevivePlayer(int playerId)
        {
            return gameStateService.RevivePlayer(playerId);
        }

        public async Task<GameStateDTO> StartTravelling(int playerId)
        {
            var state = await gameStateService.StartTravelling(playerId);

            notificationManager.SendNotification("Arrive now!", "You can arrive now!", state.StateTransitionTimeUTC);
            return state;
        }
    }
}
