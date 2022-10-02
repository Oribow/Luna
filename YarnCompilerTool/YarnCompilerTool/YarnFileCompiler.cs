using Google.Protobuf;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using Yarn;
using Yarn.Compiler;

namespace YarnCompilerTool
{
    class YarnFileCompiler
    {

        public void Compile(string file)
        {
            Log.Information("Compiling " + file);
            Yarn.Program program;
            IDictionary<string, StringInfo> defaultStringTable;
            try
            {
                string content = ReadAndCleanupFile(file);
                string fileName = Path.GetFileNameWithoutExtension(file);
                var job = new CompilationJob();
                job.CompilationType = CompilationJob.Type.FullCompilation;
                var compFile = new CompilationJob.File() { FileName = file, Source = content };
                job.Files = new CompilationJob.File[] { compFile };
                var result = Compiler.Compile(job);

                foreach (var diag in result.Diagnostics)
                {
                    Log.Error(diag.ToString());
                }
                if (result.Program == null)
                    throw new Exception();
                program = result.Program;
                defaultStringTable = result.StringTable;


            }
            catch (Exception pe)
            {
                Log.Error($"Failed to compile {file}.");
                return;
            }

            var context = new Yarn.Analysis.Context();

            Dialogue dialogue = new Dialogue(new MemoryVariableStore());
            dialogue.AddProgram(program);
            dialogue.Analyse(context);

            foreach (var diagnosis in context.FinishAnalysis())
            {
                switch (diagnosis.severity)
                {
                    case Yarn.Analysis.Diagnosis.Severity.Error:
                        Log.Error(diagnosis.ToString(showSeverity: false));
                        break;
                    case Yarn.Analysis.Diagnosis.Severity.Warning:
                        Log.Warning(diagnosis.ToString(showSeverity: false));
                        break;
                    case Yarn.Analysis.Diagnosis.Severity.Note:
                        Log.Information(diagnosis.ToString(showSeverity: false));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // write byte code
            string byteFileName = Path.ChangeExtension(file, "yrc");
            if (File.Exists(byteFileName)) File.Delete(byteFileName);
            using (FileStream f = File.OpenWrite(byteFileName))
            {
                program.WriteTo(f);
            }

            // write string table
            string stringTableFileName = Path.ChangeExtension(file, ".csv");
            if (File.Exists(stringTableFileName)) File.Delete(stringTableFileName);
            using (StreamWriter writer = new StreamWriter(File.OpenWrite(stringTableFileName)))
            {
                var lines = defaultStringTable.Select(s => $"{s.Key};{s.Value.text};");
                foreach (var line in lines)
                    writer.WriteLine(line);
            }
        }

        private string ReadAndCleanupFile(string file)
        {
            string content;
            using (var reader = File.OpenText(file))
            {
                // regex for titles with space: ^title: (.+ .+)
                // regex for where to replace: \|Confirm Jump]]
                content = reader.ReadToEnd();
            }

            var matches = Regex.Matches(content, "^title: (.+ .+)$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string nodeName = match.Groups[1].Value;
                string improvedNodeName = nodeName.Replace(' ', '_');

                content = Regex.Replace(content, $"\\|{nodeName}]]", $"|{improvedNodeName}]]");
                content = Regex.Replace(content, $"^title: {nodeName}$", $"title: {improvedNodeName}", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }

            const string optionsPattern = @"\[\[(.*?)\|(.*?)\]\]";
            var bm = Regex.Match(content, optionsPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            while (bm.Success)
            {
                string label = bm.Groups[1].Value;
                string dst = bm.Groups[2].Value;

                content = Regex.Replace(content, @$"\[\[({Regex.Escape(label)})\|({Regex.Escape(dst)})\]\]", $"-> {label}\n  <<jump {dst}>>");
                bm = Regex.Match(content, optionsPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }
            Log.Information(content);
            return content;
        }
    }
}
