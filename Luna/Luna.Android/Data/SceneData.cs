using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Luna.Biz.DataAccessors;
using Luna.Biz.DataAccessors.Scenes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarn;

namespace Luna.Droid.Data
{
    class SceneData : ISceneData
    {
        public Guid Id { get; }
        public string Name { get; }
        public string BackgroundImage { get; }
        public string PathToFolder { get; }

        private string yarnFile;


        public SceneData(Guid id, string name, string backgroundImage, string pathToFolder, string yarnFile)
        {
            Id = id;
            Name = name;
            BackgroundImage = backgroundImage;
            this.yarnFile = yarnFile;
            PathToFolder = pathToFolder;
        }

        public string ResolveAssetPath(string imageName)
        {
            return Path.Combine(PathToFolder, imageName);
        }

        public async Task<Dictionary<string, string>> GetLinesTable()
        {
            string path = ResolveAssetPath(yarnFile + ".csv");

            Dictionary<string, string> strings = new Dictionary<string, string>();

            using (var reader = File.OpenText(path))
            {
                while (!reader.EndOfStream)
                {
                    string[] parts = (await reader.ReadLineAsync()).Split(';');
                    strings.Add(parts[0], parts[1]);
                }
            }
            return strings;
        }

        public Task<Program> GetYarnProgramm()
        {
            string path = ResolveAssetPath(yarnFile + ".yrc");

            return Task.Run(() =>
            {
                using (var reader = File.OpenRead(path))
                {
                    return Program.Parser.ParseFrom(reader);
                }
            });
        }
    }
}