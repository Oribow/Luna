using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Luna.Biz.Scenes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarn;

namespace Luna.Droid.Data
{
    class Quest : IQuest
    {
        public IScene Scene => scene;

        public string Id { get; }

        public string Name { get; }

        public string StartNode { get; }

        private string yarnFile;
        private Scene scene;

        public Quest(Scene scene, string id, string name, string startNode, string yarnFile)
        {
            this.scene = scene;
            Id = id;
            Name = name;
            StartNode = startNode;
            this.yarnFile = yarnFile;
        }

        public async Task<Dictionary<string, string>> GetLinesTable()
        {
            string path = Path.Combine(scene.PathToFolder, yarnFile + ".csv");

            Dictionary<string, string> strings = new Dictionary<string, string>();

            using (var reader = File.OpenText(path))
            {
                // skip header
                reader.ReadLine();
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
            string path = Path.Combine(scene.PathToFolder, yarnFile + ".yrc");

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