using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Yarn;

namespace Luna.Biz.Scenes
{
    public interface ISceneRepository
    {
        IEnumerable<Guid> ListScenes();
        Task<IScene> Get(Guid sceneId);
    }
}
