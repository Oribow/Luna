using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Luna.Biz.Scenes;
using Luna.Biz.Scenes.Schemas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Luna.Droid.Data
{
    class SceneRepository : ISceneRepository
    {
        public static string BasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "locations");

        IDeserializer deserializer = new DeserializerBuilder()
                  .WithNamingConvention(CamelCaseNamingConvention.Instance)
                  .Build();

        public SceneRepository()
        {
            Directory.CreateDirectory(BasePath);
        }

        public Task<IScene> Get(Guid sceneId)
        {
            string path = Path.Combine(BasePath, sceneId.ToString(), "manifest.yml");
            using (var reader = File.OpenText(path))
            {
                var manifest = deserializer.Deserialize<Manifest>(reader);
                var scene = new Scene(sceneId, manifest.name, manifest.backgroundImage, manifest.introQuest);

                var quests = manifest.quests.Select(q => (IQuest)new Quest(scene, q.name, q.name, q.startNode, q.yarnFile)).ToDictionary(q => q.Id);
                scene.Quests = quests;

                return Task.FromResult<IScene>(scene);
            }
        }

        public IEnumerable<Guid> ListScenes()
        {
            return Directory.EnumerateDirectories(BasePath)
                 .Select(s => Guid.Parse(Path.GetFileName(s)));
        }
    }
}