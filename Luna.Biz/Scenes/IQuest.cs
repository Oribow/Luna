using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yarn;

namespace Luna.Biz.Scenes
{
    public interface IQuest
    {
        Task<Program> GetYarnProgramm();
        Task<Dictionary<string, string>> GetLinesTable();
    }
}
