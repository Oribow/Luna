using Luna.Biz.Scenes;
using Luna.Biz.QuestPlayer.Messages;
using Luna.Biz.Services;
using System.Collections.Generic;
using System.Linq;
using Yarn;
using static Yarn.Dialogue;
using System.Threading.Tasks;
using System.IO;
using System;
using Luna.Biz.Extensions;

namespace Luna.Biz.QuestPlayer.Yarn
{
    class YarnPlayer : IMessageSource
    {
        readonly Dialogue dialogueVM;
        readonly IReadOnlyDictionary<string, string> lines;

        private Message currentMessage;
        
        public YarnPlayer(Dictionary<string, string> lines, Program program)
        {
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
            dialogueVM.SetNode();
        }

        public void SelectOption(int index)
        {
            dialogueVM.SetSelectedOption(index);
        }

        public Task<Message> Continue()
        {
            dialogueVM.Continue();
            return Task.FromResult(currentMessage);
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

            var msg = new TextMessage(lineStr);
            currentMessage = msg;
            return HandlerExecutionType.PauseExecution;
        }

        private HandlerExecutionType HandleCommand(Command command)
        {
            var argV = command.Text.Split(' ');
            Message msg;
            switch (argV[0])
            {
                case "image":
                    msg = new ImageMessage(argV[1]);
                    break;
                case "wait":
                    TimeSpan delay = TimeSpanExtensions.ParseTwitchTime(argV[1]);
                    msg = new WaitMessage(DateTime.UtcNow + delay);
                    break;
                case "death":
                    msg = new DeathMessage();
                    break;
                default:
                    return HandlerExecutionType.ContinueExecution;
            }
            currentMessage = msg;
            return HandlerExecutionType.PauseExecution;
        }

        private void HandleOptions(OptionSet options)
        {
            var ops = options.Options.Select(
                op => new DialogueOption()
                {
                    Id = op.ID,
                    Name = lines[op.Line.ID]
                }).ToArray();

            var msg = new ChoiceMessage()
            {
                Choices = ops
            };
            currentMessage = msg;
        }

        private void HandleDialogComplete()
        {
            currentMessage = new EndOfStreamMessage();
        }

        public void DumpTo(Stream stream)
        {
            dialogueVM.DumpState(stream);
        }

        public void LoadFrom(Stream stream)
        {
            dialogueVM.LoadState(stream);
        }
    }
}
