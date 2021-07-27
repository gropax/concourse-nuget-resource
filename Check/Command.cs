using Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Check
{
    public class Command
    {
        private TextReader _stdin;
        private TextWriter _stderr;
        private TextWriter _stdout;
        public Command(TextReader stdin, TextWriter stderr, TextWriter stdout)
        {
            _stdin = stdin;
            _stderr = stderr;
            _stdout = stdout;
        }

        public async Task<bool> Run()
        {
            var inputJson = _stdin.ReadToEnd();
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
                return false;
            }

            var outputJson = JsonSerializer.Serialize(output);
            Log($"STDOUT: {outputJson}");
            _stdout.WriteLine(outputJson);
            return true;
        }

        private void Log(string message)
        {
            _stderr.WriteLine(message);
        }
    }
}
