using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class InTests
    {
        private string _targetDir;
        public InTests()
        {
            _targetDir = Path.GetFullPath("./packages");
            if (Directory.Exists(_targetDir))
                Directory.Delete(_targetDir, recursive: true);

            Directory.CreateDirectory(_targetDir);
        }

        [Fact]
        public async Task TestCommand()
        {
            var stdin = new StringReader(@"
            {
                ""source"": {
                    ""uri"": ""https://api.nuget.org/v3/index.json"",
                    ""package_id"": ""Newtonsoft.Json""
                },
                ""version"": {
                    ""version"": ""12.0.3""
                }
            }");

            var stderrBuilder = new StringBuilder();
            var stderr = new StringWriter(stderrBuilder);

            var stdoutBuilder = new StringBuilder();
            var stdout = new StringWriter(stdoutBuilder);

            var cmd = new In.Command(stdin, stderr, stdout, _targetDir);
            bool success = await cmd.Run();

            string debug = stderr.ToString();

            Assert.True(success);
            Assert.False(string.IsNullOrWhiteSpace(stdout.ToString()));

            var package = Path.Combine(_targetDir, "Newtonsoft.Json.12.0.3.nupkg");
            Assert.True(File.Exists(package));
        }
    }
}
