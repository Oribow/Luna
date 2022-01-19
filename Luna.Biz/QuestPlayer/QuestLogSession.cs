using Luna.Biz.Models;
using Luna.Biz.QuestPlayer.Messages;
using Luna.Biz.DataAccessors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Biz.QuestPlayer
{
    class QuestLogSession : IQuestLogSession
    {
        IDbContextFactory<LunaContext> contextFactory;
        int questLogId;
        IMessageSource messageSource;
        ISceneData scene;
        MessageSerializer messageSerializer;

        public QuestLogSession(IDbContextFactory<LunaContext> contextFactory, int questLogId, IMessageSource messageSource, MessageSerializer messageSerializer, ISceneData scene)
        {
            this.contextFactory = contextFactory;
            this.questLogId = questLogId;
            this.messageSource = messageSource;
            this.messageSerializer = messageSerializer;
            this.scene = scene;
        }

        public async Task<IEnumerable<Message>> GetHistory()
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var qms = await context.QuestMessages
                    .Where(qm => qm.QuestLogId == questLogId)
                    .OrderBy(qm => qm.Id)
                    .ToArrayAsync();

                return qms.Select(qm => messageSerializer.Deserialize(qm));
            }
        }

        public Task<Message> Continue()
        {
            return messageSource.Continue();
        }

        public void SelectOption(int option)
        {
            messageSource.SelectOption(option);
        }

        public string ResolveAssetPath(string assetName)
        {
            return scene.ResolveAssetPath(assetName);
        }

        public async Task SaveNewMessage(Message msg)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var qm = messageSerializer.Serialize(msg, questLogId);
                context.QuestMessages.Add(qm);

                var questLog = await context.QuestLogs.FindAsync(questLogId);
                using (var stream = new MemoryStream())
                {
                    messageSource.DumpTo(stream);
                    questLog.DialogState = stream.ToArray();

                    Debug.WriteLine(Encoding.Default.GetString(questLog.DialogState));
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task SaveExistingMessage(Message msg)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var qm = messageSerializer.Serialize(msg, questLogId);
                var latestMsg = await context.QuestMessages.OrderByDescending(qm => qm.Id)
                    .FirstAsync();
                qm.Id = latestMsg.Id;
                context.Entry<QuestMessage>(latestMsg).CurrentValues.SetValues(qm);
                await context.SaveChangesAsync();
            }
        }
    }
}
