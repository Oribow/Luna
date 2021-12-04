using Luna.Biz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz
{
    public abstract class LunaContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<VisitedScene> VisitedScenes { get; set; }

        protected LunaContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
