using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yarn;

namespace Luna.Biz.Scenes
{
    public interface IQuest
    {
        IScene Scene { get; }
        string Id { get; }
        string Name { get; }
        string StartNode { get; }

        Task<Program> GetYarnProgramm();
        Task<Dictionary<string, string>> GetLinesTable();
    }
}
