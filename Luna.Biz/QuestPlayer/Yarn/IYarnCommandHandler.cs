using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.QuestPlayer.Yarn
{
    public interface IYarnCommandHandler
    {
        Task ActivateQuest(int questId, TimeSpan delay);
        string ResolvePath(string localPath);
    }
}
