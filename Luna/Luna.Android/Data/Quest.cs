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
        private string yarnFile;

        public Quest(string yarnFile)
        {
            this.yarnFile = yarnFile;
        }

        public async Task<Dictionary<string, string>> GetLinesTable()
        {
            string path = yarnFile + ".csv";

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
            string path = yarnFile + ".yrc";

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