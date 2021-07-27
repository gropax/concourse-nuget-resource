using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Check
{
    public class Command : CommandBase
    {
        public Command(TextReader stdin, TextWriter stderr, TextWriter stdout)
            : base(stdin, stderr, stdout) { }


        public async Task<bool> Run()
        {
            var input = ReadStdin<CheckInputDto>();
            VersionDto[] output = null;

            try
            {
                var repo = new NugetRepository(input.Source.Uri);
                var newVersions = await repo.GetNewPackageVersions(input.Source.PackageId, input.Version.Version);
                output = newVersions.Select(v => new VersionDto()
                    {
                        PackageId = input.Version.PackageId,
                        Version = v,
                    }).ToArray();
            }
            catch (Exception e)
            {
                LogError(e);
                return false;
            }

            WriteStdout(output);
            return true;
        }
    }
}
