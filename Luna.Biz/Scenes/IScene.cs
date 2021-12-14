using Luna.Biz.Scenes.Schemas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Yarn;

namespace Luna.Biz.Scenes
{
    public interface IScene
    {
        Guid Id { get; }
        string Name { get; }
        string BackgroundImage { get; }
        IQuest Quest { get; }

        string ResolveAssetPath(string name);
    }
}
