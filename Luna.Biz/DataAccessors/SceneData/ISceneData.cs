using Luna.Biz.DataAccessors.Scenes.Schemas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Yarn;

namespace Luna.Biz.DataAccessors.Scenes
{
    public interface ISceneData
    {
        Guid Id { get; }
        string Name { get; }
        string BackgroundImage { get; }

        string ResolveAssetPath(string name);
        Task<Program> GetYarnProgramm();
        Task<Dictionary<string, string>> GetLinesTable();
    }
}
