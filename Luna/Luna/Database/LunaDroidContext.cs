using Luna.Biz;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Database
{
    class LunaDroidContext : LunaContext
    {
        public LunaDroidContext(DbContextOptions<LunaContext> options) : base(options)
        {
            SQLitePCL.Batteries_V2.Init();
        }
    }
}
