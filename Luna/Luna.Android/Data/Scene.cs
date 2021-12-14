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

        public string PathToFolder { get; }

        public IQuest Quest { get; }

        public Scene(Guid id, string name, string backgroundImage, IQuest quest)
        {
            Id = id;
            Name = name;
            BackgroundImage = backgroundImage;
            Quest = quest;

            PathToFolder = Path.Combine(SceneRepository.BasePath, id.ToString());
        }

        public string ResolveAssetPath(string imageName)
        {
            return Path.Combine(PathToFolder, imageName);
        }
    }
}