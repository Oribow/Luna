using Luna.Biz.Scenes;
using Luna.Biz.QuestPlayer.Instructions;
using Luna.Biz.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Yarn;
using static Yarn.Dialogue;

namespace Luna.Biz.QuestPlayer.Yarn
{
    public class YarnPlayer : IQuestPlayer
    {
        public bool AutoContinue => dialogueVM.CurrentOpCode == Instruction.Types.OpCode.AddOption;

        readonly Dialogue dialogueVM;
        readonly IQuest quest;
        readonly QuestService questService;
        readonly int playerId;
        readonly IReadOnlyDictionary<string, string> lines;

        InstructionDTO currentMessage;

        public YarnPlayer(IQuest quest, QuestService questService, int playerId, Dictionary<string, string> lines, Program program)
        {
            this.quest = quest;
            this.questService = questService;
            this.playerId = playerId;

            dialogueVM = new Dialogue(new MemoryVariableStore())
            {
                lineHandler = HandleLine,
                commandHandler = HandleCommand,
                optionsHandler = HandleOptions,
                nodeStartHandler = HandleNodeStart,
                nodeCompleteHandler = (node) => HandlerExecutionType.ContinueExecution,
                dialogueCompleteHandler = HandleDialogComplete,
                LogDebugMessage = (message) => { },
                LogErrorMessage = (message) => { },
            };

            this.lines = lines;
            dialogueVM.SetProgram(program);
            dialogueVM.SetNode(quest.StartNode);
        }

        public void SelectOption(int index)
        {
            dialogueVM.SetSelectedOption(index);
        }

        public InstructionDTO NextInstruction()
        {
            dialogueVM.Continue();
            return dialogueVM.IsActive ? currentMessage : null;
        }

        private HandlerExecutionType HandleNodeStart(string startedNodeName)
        {
            return HandlerExecutionType.ContinueExecution;
        }

        private HandlerExecutionType HandleLine(Line line)
        {
            var lineStr = lines[line.ID];
            if (lineStr.Contains(":"))
            {
                int semiIndex = lineStr.IndexOf(':');
                if (lineStr.Length > semiIndex + 1)
                    lineStr = lineStr.Substring(semiIndex + 1).Trim();
            }
            currentMessage = new TextMessageDTO()
            {
                Text = lineStr
            };
            return HandlerExecutionType.PauseExecution;
        }

        private HandlerExecutionType HandleCommand(Command command)
        {
            var argV = command.Text.Split(' ');

            switch (argV[0])
            {
                case "image":
                    currentMessage = new ImageMessageDTO()
                    {
                        ImagePath = quest.Scene.GetImagePath(argV[1])
                    };
                    return HandlerExecutionType.PauseExecution;
                case "queue":
                    TimeSpan delay;
                    if (argV.Length == 3)
                    {
                        delay = TimeSpan.Parse(argV[2], CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        delay = TimeSpan.Zero;
                    }
                    _ = questService.ActivateQuest(quest.Scene.Id, argV[1], playerId);
                    break;
            }
            return HandlerExecutionType.ContinueExecution;
        }

        private void HandleOptions(OptionSet options)
        {
            var ops = options.Options.Select(
                op => new DialogueOptionDTO()
                {
                    Id = op.ID,
                    Name = lines[op.Line.ID]
                }).ToArray();

            currentMessage = new ChoiceMessageDTO()
            {
                Choices = ops
            };
        }

        private void HandleDialogComplete()
        {

        }
    }
}
