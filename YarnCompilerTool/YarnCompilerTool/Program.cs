using CommandLine;
using Google.Protobuf;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Yarn;
using Yarn.Compiler;

namespace YarnCompilerTool {
    class Program {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            new StoryProcessor().Process();
        }
    }
}
