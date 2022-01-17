using Luna.Biz;
using Luna.Biz.Models;
using Luna.Biz.Services;
using Microsoft.EntityFrameworkCore;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Database
{
    public class DatabaseCreator
    {
        public static string DBPath;
        public static string ConnectionString;

        static DatabaseCreator()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            DBPath = Path.Combine(folder, "luna.sqlite");
            ConnectionString = $"Filename={DBPath};Mode=ReadWrite;";
        }

        public static async Task RecreateDatabase(IDbContextFactory<LunaContext> contextFactory)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                if (File.Exists(DBPath))
                    await context.Database.EnsureDeletedAsync();
                EnsureDatabaseFileExists();
                await context.Database.EnsureCreatedAsync();
            }
        }

        public static async Task EnsureDatabaseExists(IDbContextFactory<LunaContext> contextFactory)
        {
            EnsureDatabaseFileExists();
            using (var context = contextFactory.CreateDbContext())
            {
                await context.Database.EnsureCreatedAsync();
            }
        }

        private static void EnsureDatabaseFileExists()
        {
            string folder = Path.GetDirectoryName(DBPath);
            Directory.CreateDirectory(folder);

            if (!File.Exists(DBPath))
                File.Create(DBPath);
        }
    }
}
