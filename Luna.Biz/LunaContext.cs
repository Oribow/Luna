using Luna.Biz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz
{
    public class LunaContext : DbContext
    {
        internal DbSet<Player> Players { get; set; }
        internal DbSet<QuestLog> QuestLogs { get; set; }
        internal DbSet<QuestMessage> QuestMessages { get; set; }
        internal DbSet<RevealedScene> RevealedScenes { get; set; }


        protected LunaContext(DbContextOptions options) : base(options)
        {
        }
    }
}
