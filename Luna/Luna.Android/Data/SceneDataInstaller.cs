using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Luna.Droid.Data
{
    class SceneDataInstaller
    {
        public static string BasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sceneData");

        public static bool UpdateDataFrom(Stream archiveStream)
        {
            int installedVersion = ReadInstalledVersion();
            using (var archive = new ZipArchive(archiveStream))
            {
                int version = CheckArchiveVersion(archive);

                if (version > installedVersion)
                {
                    ExtractToDirectory(archive);
                    return true;
                }
            }
            return false;
        }

        public static int ReadInstalledVersion()
        {
            string versionFile = Path.Combine(BasePath, "version.txt");
            int version = -1;
            if (File.Exists(versionFile))
            {
                string content = File.ReadAllText(versionFile);
                if (int.TryParse(content, out int readVersion))
                {
                    version = readVersion;
                }
            }
            return version;
        }

        private static int CheckArchiveVersion(ZipArchive archive)
        {
            using (var stream = archive.GetEntry("version.txt").Open())
            using (var reader = new StreamReader(stream))
            {
                string content = reader.ReadToEnd();
                return int.Parse(content);
            }
        }

        private static void ExtractToDirectory(ZipArchive archive)
        {
            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.Combine(BasePath, file.FullName);
                Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                
                using (var streamIn = file.Open())
                using (var streamOut = File.OpenWrite(completeFileName))
                {
                    streamIn.CopyTo(streamOut);
                }
            }
        }
    }
}