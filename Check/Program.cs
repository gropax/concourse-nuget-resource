using Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Check
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var inputJson = Console.In.ReadToEnd();
            Log($"STDIN: {inputJson}");
            var input = JsonSerializer.Deserialize<CheckDto>(inputJson);

            var repo = new NugetRepository(input.Source.Uri);

            VersionDto[] output = null;

            try
            {
                var newVersions = await repo.GetNewPackageVersions(input.Version.PackageId, input.Version.Version);

                output = newVersions.Select(v => new VersionDto()
                    {
                        PackageId = input.Version.PackageId,
                        Version = v,
                    }).ToArray();
            }
            catch (Exception e)
            {
                Log($"Error: {e.GetType()} : {e.Message}");
            }

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
