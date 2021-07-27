using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common
{
    public abstract class CommandBase
    {
        protected TextReader _stdin;
        protected TextWriter _stderr;
        protected TextWriter _stdout;

        protected CommandBase(TextReader stdin, TextWriter stderr, TextWriter stdout)
        {
            _stdin = stdin;
            _stderr = stderr;
            _stdout = stdout;
        }

        protected T ReadStdin<T>()
        {
            var inputJson = _stdin.ReadToEnd();
            Log($"STDIN: {inputJson}");
            return JsonSerializer.Deserialize<T>(inputJson);
        }

        protected void WriteStdout<T>(T output)
        {
            var outputJson = JsonSerializer.Serialize(output);
            Log($"STDOUT: {outputJson}");
            _stdout.WriteLine(outputJson);
        }

        protected void LogError(Exception e)
        {
            Log($"Error: {e.GetType()} : {e.Message} \n {e.StackTrace}");
        }

        protected void Log(string message)
        {
            _stderr.WriteLine(message);
        }
    }
}
