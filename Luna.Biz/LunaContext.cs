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
        internal DbSet<AssignedScene> AssignedScenes { get; set; }
        internal DbSet<QuestLog> QuestLogs { get; set; }
        internal DbSet<QuestMessage> QuestMessages { get; set; }


        protected LunaContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>().HasMany<AssignedScene>().WithOne(sc => sc.Player);
            modelBuilder.Entity<Player>().HasOne(pl => pl.CurrentScene).WithOne().HasForeignKey<Player>(pl => pl.CurrentSceneId).IsRequired(false);
            modelBuilder.Entity<Player>().HasOne(pl => pl.PrevScene).WithOne().HasForeignKey<Player>(pl => pl.PrevSceneId).IsRequired(false);
        }
    }
}
