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

namespace Luna.Droid.Data
{
    class Scene : IScene
    {
        public Guid Id { get; }

        public string Name { get; }

        public string BackgroundImage { get; }

        public string IntroQuest { get; }

        public Dictionary<string, IQuest> Quests { get; internal set; }
        public string PathToFolder { get; }

        public Scene(Guid id, string name, string backgroundImage, string introQuest)
        {
            Id = id;
            Name = name;
            BackgroundImage = backgroundImage;
            IntroQuest = introQuest;

            PathToFolder = Path.Combine(SceneRepository.BasePath, id.ToString());
        }

        public Task<string> GetImagePath(string imageName)
        {
            return Task.FromResult(Path.Combine(PathToFolder, imageName));
        }
    }
}