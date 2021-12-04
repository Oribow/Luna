using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Data
{
    public class LunaContextFactory : IDbContextFactory<LunaContext>
    {
        string connectionString;

        public LunaContextFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public LunaContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<LunaContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new LunaContext(optionsBuilder.Options);
        }
    }
}
