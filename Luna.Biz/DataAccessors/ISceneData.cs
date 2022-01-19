using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Yarn;

namespace Luna.Biz.DataAccessors
{
    public interface ISceneData
    {
        Guid Id { get; }
        string Name { get; }
        string BackgroundImage { get; }
        Vector2 Position { get; }

        string ResolveAssetPath(string imageName);
        Task<Program> GetYarnProgramm();
        Task<Dictionary<string, string>> GetLinesTable();
    }
}
