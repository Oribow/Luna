using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Yarn;

namespace Luna.Biz.DataAccessors
{
    public interface ISceneDataRepository
    {
        int Version { get; }
        IEnumerable<Guid> ListScenes();
        Task<ISceneData> GetSceneData(Guid sceneId);
        int SceneCount { get; }
    }
}
