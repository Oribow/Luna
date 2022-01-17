using Luna.Biz.DataAccessors;
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
using Luna.Biz.DataAccessors.Scenes;

namespace Luna.Biz.Services
{
    public class QuestLogService
    {
        IDbContextFactory<LunaContext> contextFactory;
        ISceneDataRepository sceneRepo;
        MessageSerializer messageSerializer;
        SceneService sceneService;

        internal QuestLogService(IDbContextFactory<LunaContext> contextFactory, ISceneDataRepository sceneRepo, MessageSerializer messageSerializer, SceneService sceneService)
        {
            this.contextFactory = contextFactory;
            this.sceneRepo = sceneRepo;
            this.messageSerializer = messageSerializer;
            this.sceneService = sceneService;
        }

        public async Task<IQuestLogSession> GetOrCreateQuestLogSession(int playerId, int sceneId)
        {
            var sceneDataInfo = await sceneService.GetSceneDataInfo(sceneId);
            var scene = await sceneRepo.GetSceneData(sceneDataInfo.Id);

            var lines = await scene.GetLinesTable();
            var program = await scene.GetYarnProgramm();

            QuestLog questLog;
            using (var context = contextFactory.CreateDbContext())
            {
                questLog = await context.QuestLogs.Where(q => q.SceneDataId == sceneDataInfo.Id && q.PlayerId == playerId).FirstOrDefaultAsync();

                if (questLog == null)
                {
                    context.QuestLogs.Add(new QuestLog(playerId, sceneDataInfo.Id));
                    await context.SaveChangesAsync();
                    questLog = await context.QuestLogs.Where(q => q.SceneDataId == sceneDataInfo.Id && q.PlayerId == playerId).FirstOrDefaultAsync();
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
