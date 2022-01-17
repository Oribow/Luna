using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Luna.Biz.DataAccessors;
using Luna.Biz.DataAccessors.Scenes;
using Luna.Biz.DataAccessors.Scenes.Schemas;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Luna.Droid.Data
{
    class SceneDataRepository : ISceneDataRepository
    {
        IDeserializer deserializer = new DeserializerBuilder()
                  .WithNamingConvention(CamelCaseNamingConvention.Instance)
                  .Build();

        Guid[] storyIds = new Guid[0];
        Dictionary<Guid, ISceneData> sceneCache = new Dictionary<Guid, ISceneData>();

        public int Version { get; private set; } = -1;

        public int SceneCount => storyIds.Length;

        public Task<ISceneData> GetSceneData(Guid id)
        {
            if (sceneCache.TryGetValue(id, out ISceneData scene))
                return Task.FromResult(scene);

            scene = LoadSceneData(id);
            sceneCache.Add(id, scene);
            return Task.FromResult(scene);
        }

        public IEnumerable<Guid> ListScenes()
        {
            return Directory.EnumerateDirectories(SceneDataInstaller.BasePath)
                 .Select(s => Guid.Parse(Path.GetFileName(s)));
        }

        public void RefreshData()
        {
            sceneCache = new Dictionary<Guid, ISceneData>();
            // create dir if it doesnt exist yet
            Directory.CreateDirectory(SceneDataInstaller.BasePath);

            Version = SceneDataInstaller.ReadInstalledVersion();

            if (Version == -1)
                return;
            
            // read scene ids
            storyIds = Directory.EnumerateDirectories(SceneDataInstaller.BasePath)
                 .Select(s => Guid.Parse(Path.GetFileName(s)))
                 .ToArray();
        }

        private ISceneData LoadSceneData(Guid id)
        {
            string packagePath = Path.Combine(SceneDataInstaller.BasePath, id.ToString());
            string path = Path.Combine(packagePath, "manifest.yml");
            using (var reader = File.OpenText(path))
            {
                var manifest = deserializer.Deserialize<Manifest>(reader);
                var scene = new SceneData(id, manifest.name, manifest.backgroundImage, packagePath, manifest.quest.yarnFile);

                return scene;
            }
        }
    }
}