using Luna.Biz.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.Services
{
    public interface IPlayerService
    {
        Task CreatePlayer(int id);
        Task<bool> DoesPlayerExist(int id);
        Task<PlayerStateDTO> GetPlayersState(int playerId);
        Task<PlayerStateDTO> LetPlayerTravelTo(int playerId, int sceneId);
        Task RevivePlayer(int playerId);
        Task<PlayerStateDTO> KillPlayer(int playerId);
    }
}
