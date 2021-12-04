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

namespace Luna.Biz.Services
{
    public class QuestService
    {
        IDbContextFactory<LunaContext> contextFactory;
        ISceneRepository sceneRepo;

        internal QuestService(IDbContextFactory<LunaContext> contextFactory, ISceneRepository sceneRepo)
        {
            this.contextFactory = contextFactory;
            this.sceneRepo = sceneRepo;
        }

        public async Task CompleteQuest(Guid locId, string questId, int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var scene = await context.VisitedScenes.Where(v => v.PlayerId == playerId && v.LocationId == locId).Include(v => v.AvailableQuests).FirstAsync();

                int index = scene.AvailableQuests.FindIndex(a => a.QuestId == questId);
                if (index != -1)
                {
                    scene.AvailableQuests.RemoveAt(index);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<QuestDTO[]> ListAvailableQuests(int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var player = await context.Players.FindAsync(playerId);

                if (player.IsTraveling)
                    throw new InvalidOperationException("Player has no quests when travelling");

                var availableQuestIds = await context.VisitedScenes
                    .Where(v => v.PlayerId == playerId && v.LocationId == player.CurrentSceneId)
                    .Select(v => v.AvailableQuests).FirstAsync();

                var scene = await sceneRepo.Get(player.CurrentSceneId.Value);

                return availableQuestIds.Select(a =>
                {
                    var quest = scene.Quests[a.QuestId];
                    return new QuestDTO(scene.Id, quest.Id, quest.Name);
                }).ToArray();
            }
        }

        public async Task<PlayableQuestDTO> StartQuest(Guid locId, string questId, int playerId)
        {
            var scene = await sceneRepo.Get(locId);
            var quest = scene.Quests[questId];

            var lines = await quest.GetLinesTable();
            var program = await quest.GetYarnProgramm();

            var player = new YarnPlayer(quest, this, playerId, lines, program);
            return new PlayableQuestDTO(locId, questId, quest.Name, player);
        }

        public async Task ActivateQuest(Guid locId, string questId, int playerId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var scene = await context.VisitedScenes.Where(v => v.PlayerId == playerId && v.LocationId == locId).Include(v => v.AvailableQuests).FirstAsync();

                var quest = scene.AvailableQuests.Find(a => a.QuestId == questId);
                if (quest == null)
                {
                    scene.AvailableQuests.Add(new AvailableQuest(questId));
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
