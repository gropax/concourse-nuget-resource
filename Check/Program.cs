using Resource;
using System;
using System.Text.Json;

namespace Check
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputJson = Console.In.ReadToEnd();
            Log($"STDIN: {inputJson}");
            var input = JsonSerializer.Deserialize<CheckDto>(inputJson);

            VersionDto[] output = new VersionDto[0];


            var outputJson = JsonSerializer.Serialize(output);
            Log($"STDOUT: {outputJson}");
            Console.Out.WriteLine(outputJson);
        }


        private static void Log(string message)
        {
            Console.Error.WriteLine(message);
        }
    }
}
