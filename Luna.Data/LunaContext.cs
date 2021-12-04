using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Data
{
    public class LunaContext : DbContext
    {
        
        public LunaContext(DbContextOptions<LunaContext> options) : base(options)
        {
            SQLitePCL.Batteries_V2.Init();
            this.Database.EnsureCreated();
        }
    }
}
