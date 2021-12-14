using Luna.Biz.Scenes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luna.Biz.QuestPlayer.Yarn;
using Luna.Biz.DataTransferObjects;
using Luna.Biz.Models;
using Luna.Biz.QuestPlayer;
using System.IO;

namespace Luna.Biz.Services
{
    public class QuestLogService
    {
        IDbContextFactory<LunaContext> contextFactory;
        ISceneRepository sceneRepo;
        MessageSerializer messageSerializer;

        internal QuestLogService(IDbContextFactory<LunaContext> contextFactory, ISceneRepository sceneRepo, MessageSerializer messageSerializer)
        {
            this.contextFactory = contextFactory;
            this.sceneRepo = sceneRepo;
            this.messageSerializer = messageSerializer;
        }

        public async Task<IQuestLogSession> GetOrCreateQuestLogSession(Guid locId, int playerId)
        {
            var scene = await sceneRepo.Get(locId);
            var quest = scene.Quest;

            var lines = await quest.GetLinesTable();
            var program = await quest.GetYarnProgramm();

            QuestLog questLog;
            using (var context = contextFactory.CreateDbContext())
            {
                questLog = await context.QuestLogs.Where(q => q.LocationId == locId && q.PlayerId == playerId).FirstOrDefaultAsync();

                if (questLog == null)
                {
                    context.QuestLogs.Add(new QuestLog(playerId, locId));
                    await context.SaveChangesAsync();
                    questLog = await context.QuestLogs.Where(q => q.LocationId == locId && q.PlayerId == playerId).FirstOrDefaultAsync();
                }
            }

            var msgSource = new YarnPlayer(lines, program);
            if (questLog.DialogState != null)
            {
                using (var stream = new MemoryStream(questLog.DialogState))
                    msgSource.LoadFrom(stream);
            }
            var questLogSession = new QuestLogSession(contextFactory, questLog.Id, msgSource, messageSerializer, scene);
           
            return questLogSession;
        }
    }
}
