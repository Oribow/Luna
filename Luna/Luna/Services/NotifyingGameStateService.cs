using Luna.Biz.DataTransferObjects;
using Luna.Biz.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Services
{
    class NotifyingGameStateService : IPlayerService
    {
        readonly PlayerService gameStateService;
        readonly INotificationManager notificationManager;

        public NotifyingGameStateService(PlayerService gameStateService, INotificationManager notificationManager) : base()
        {
            this.gameStateService = gameStateService;
            this.notificationManager = notificationManager;
        }

        public Task CreatePlayer(int id)
        {
            return ((IPlayerService)gameStateService).CreatePlayer(id);
        }

        public Task<bool> DoesPlayerExist(int id)
        {
            return ((IPlayerService)gameStateService).DoesPlayerExist(id);
        }

        public Task<PlayerStateDTO> GetPlayersState(int playerId)
        {
            return ((IPlayerService)gameStateService).GetPlayersState(playerId);
        }

        public async Task<PlayerStateDTO> KillPlayer(int playerId)
        {
            var state = await gameStateService.KillPlayer(playerId);

            notificationManager.SendNotification("Revive now!", "You can revive now!", state.LockoutEndUTC);
            return state;
        }

        public async Task<PlayerStateDTO> LetPlayerTravelTo(int playerId, int sceneId)
        {
            var state = await gameStateService.LetPlayerTravelTo(playerId, sceneId);

            notificationManager.SendNotification("Arrive now!", "You can arrive now!", state.LockoutEndUTC);
            return state;
        }

        public Task RevivePlayer(int playerId)
        {
            return ((IPlayerService)gameStateService).RevivePlayer(playerId);
        }
    }
}
