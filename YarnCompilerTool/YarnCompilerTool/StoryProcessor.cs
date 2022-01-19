using Luna.Schemas;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YarnCompilerTool
{
    class StoryProcessor
    {

        IDeserializer deserializer = new DeserializerBuilder()
                  .WithNamingConvention(CamelCaseNamingConvention.Instance)
                  .Build();
        ISerializer serializer = new SerializerBuilder()
                  .WithNamingConvention(CamelCaseNamingConvention.Instance)
                  .Build();

        public void Process()
        {
            PatchManifestFiles();
            // compile all yarn files you find
            CompileYarnFiles();
            PackageFiles();
        }

        void CompileYarnFiles()
        {
            YarnFileCompiler yarnFileCompiler = new YarnFileCompiler();
            foreach (var yarnFile in Directory.EnumerateFiles(".", "*.yarn", SearchOption.AllDirectories))
            {
                yarnFileCompiler.Compile(yarnFile);
            }
        }

        void PackageFiles()
        {
            using (var fs = File.Open("./testdata.zip", FileMode.Create, FileAccess.ReadWrite))
            using (var archive = new ZipArchive(fs, ZipArchiveMode.Create))
            {

                foreach (var dir in Directory.EnumerateDirectories("."))
                {
                    Guid id = GetGuid(dir);

                    foreach (var file in Directory.EnumerateFiles(dir))
                    {

                        if (!ShouldFileBeCopied(file))
                            continue;

                        archive.CreateEntryFromFile(file,
                            Path.Combine(id.ToString(), Path.GetFileName(file)), CompressionLevel.Optimal);
                    }
                }

                archive.CreateEntryFromFile("./version.txt", "version.txt");
            }
        }

        Guid GetGuid(string folder)
        {
            string path = Path.Combine(folder, "id");
            if (File.Exists(path))
            {
                string val = File.ReadAllText(path);
                return Guid.Parse(val);
            }

            Guid id = Guid.NewGuid();
            File.WriteAllText(path, id.ToString());
            return id;
        }

        bool ShouldFileBeCopied(string file)
        {
            return !file.EndsWith(".yarn") && !file.EndsWith("id");
        }

        List<Location> LoadLocations()
        {
            var positions = new List<Location>();

            using (var stream = File.OpenRead("./positions.csv"))
            using (var reader = new StreamReader(stream))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    var parts = line.Split("\t");

                    float x = float.Parse(parts[0]);
                    float y = float.Parse(parts[1]);

                    bool wasTaken;
                    if (parts.Length > 2)
                        wasTaken = bool.Parse(parts[2]);
                    else
                        wasTaken = false;

                    positions.Add(new Location(new Vector2(x, y), wasTaken));
                }
            }
            Log.Information($"Loaded {positions.Count} positions.");
            return positions;
        }

        void WriteLocations(List<Location> locations)
        {
            using (var stream = File.OpenWrite("./positions.csv"))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine("x\ty\tfree");
                foreach (var loc in locations)
                {
                    writer.WriteLine($"{loc.Position.X}\t{loc.Position.Y}\t{loc.WasTaken}");
                }
            }
        }

        void PatchManifestFiles()
        {
            List<Location> positions = LoadLocations();

            foreach (var dir in Directory.EnumerateDirectories("."))
            {
                string manifestFile = Path.Combine(dir, "manifest.yml");
                Manifest manifest;
                if (File.Exists(manifestFile))
                {
                    using (var reader = File.OpenText(manifestFile))
                    {
                        manifest = deserializer.Deserialize<Manifest>(reader);
                    }
                }
                else
                {
                    manifest = new Manifest();
                }

                PatchManifestFile(manifest, positions);

                using (var stream = File.OpenWrite(manifestFile))
                using (var writer = new StreamWriter(stream))
                {
                    serializer.Serialize(writer, manifest);
                }
            }

            WriteLocations(positions);
        }

        void PatchManifestFile(Manifest manifest, List<Location> positions)
        {
            if (manifest.posX == 0 && manifest.posY == 0)
            {
                var freeLoc = positions.Where(pos => !pos.WasTaken).FirstOrDefault();

                if (freeLoc != null)
                {
                    manifest.posX = freeLoc.Position.X;
                    manifest.posY = freeLoc.Position.Y;
                    freeLoc.WasTaken = true;
                }
            }
        }
    }
}
