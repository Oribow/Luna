using Luna.Biz;
using Luna.Biz.Models;
using Microsoft.EntityFrameworkCore;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Data
{
    public class Bootstrapper
    {
        public static string DBPath;
        public static string ConnectionString;

        static Bootstrapper()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            DBPath = Path.Combine(folder, "luna.sqlite");
            ConnectionString = $"Filename={DBPath};Mode=ReadWrite;";
        }

        readonly IPlatformBootstrapHelper platformBootstrapHelper;

        public Bootstrapper(IPlatformBootstrapHelper platformBootstrapHelper)
        {
            this.platformBootstrapHelper = platformBootstrapHelper;
        }

        public async Task Bootstrap()
        {
            var contextFactory = new LunaContextFactory(ConnectionString);

            await RecreateDatabase(contextFactory);
            using (var context = contextFactory.CreateDbContext())
            {

                var first = await context.Players.FirstOrDefaultAsync();
                if (first != null)
                    return;

                Player player = new Player(App.PlayerId);
                context.Players.Add(player);
                await context.SaveChangesAsync();
            }

            if (!Directory.Exists(platformBootstrapHelper.LocPackageDir)
                || !Directory.EnumerateDirectories(platformBootstrapHelper.LocPackageDir).Any())
            {
                Directory.CreateDirectory(platformBootstrapHelper.LocPackageDir);

                using (var stream = platformBootstrapHelper.OpenTestPackage())
                using(var archive = new ZipArchive(stream))
                {
                    archive.ExtractToDirectory(platformBootstrapHelper.LocPackageDir, true);
                }
            }
        }

        private async Task RecreateDatabase(IDbContextFactory<LunaContext> contextFactory)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                if (File.Exists(DBPath))
                    await context.Database.EnsureDeletedAsync();
                EnsureDatabaseFileExists();
                await context.Database.EnsureCreatedAsync();
            }
        }

        private async Task CreateDatabase(IDbContextFactory<LunaContext> contextFactory)
        {
            EnsureDatabaseFileExists();
            using (var context = contextFactory.CreateDbContext())
            {
                await context.Database.EnsureCreatedAsync();
            }
        }

        private void EnsureDatabaseFileExists()
        {
            string folder = Path.GetDirectoryName(DBPath);
            Directory.CreateDirectory(folder);

            if (!File.Exists(DBPath))
                File.Create(DBPath);
        }
    }
}
