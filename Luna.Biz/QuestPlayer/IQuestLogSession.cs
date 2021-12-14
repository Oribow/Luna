﻿using Luna.Biz.QuestPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.QuestPlayer
{
    public interface IQuestLogSession
    {
        Task<Message> Continue();
        void SelectOption(int option);
        Task<IEnumerable<Message>> GetHistory();
        string ResolveAssetPath(string assetName);
        Task SaveCompletedMessage(Message msg);
        Task SaveStartedMessage(Message msg);
    }
}