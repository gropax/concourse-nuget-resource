using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class CheckTests
    {
        [Fact]
        public async Task TestCommand()
        {
            var stdin = new StringReader(@"
            {
                ""source"": {
                    ""uri"": ""https://api.nuget.org/v3/index.json""
                },
                ""version"": {
                    ""package_id"": ""Newtonsoft.Json"",
                    ""version"": ""12.0.3""
                }
            }");

            var stderrBuilder = new StringBuilder();
            var stderr = new StringWriter(stderrBuilder);

            var stdoutBuilder = new StringBuilder();
            var stdout = new StringWriter(stdoutBuilder);

            var cmd = new Check.Command(stdin, stderr, stdout);
            bool success = await cmd.Run();

            Assert.True(success);
            Assert.False(string.IsNullOrWhiteSpace(stdout.ToString()));
        }
    }
}
