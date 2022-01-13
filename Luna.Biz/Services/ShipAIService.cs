using Luna.Biz.ShipAISystems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.Services
{
    public class ShipAIService
    {
        IDbContextFactory<LunaContext> contextFactory;

        public ShipAIService(IDbContextFactory<LunaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public Task<ShipAI> GetShipAI(int playerId)
        {
            return Task.FromResult(new ShipAI());
        }
    }
}
