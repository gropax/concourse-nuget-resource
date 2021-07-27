using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace In
{
    public class Command : CommandBase
    {
        private string _targetDir;
        public Command(TextReader stdin, TextWriter stderr, TextWriter stdout, string targetDir)
            : base(stdin, stderr, stdout)
        {
            _targetDir = targetDir;
        }


        public async Task<bool> Run()
        {
            var input = ReadStdin<InInputDto>();

            try
            {
                var repo = new NugetRepository(input.Source.Uri);
                var file = await repo.DownloadPackage(input.Source.PackageId, input.Version.Version, _targetDir);
            }
            catch (Exception e)
            {
                LogError(e);
                return false;
            }

            WriteStdout(new InOutputDto()
            {
                Version = new VersionDto()
                {
                    PackageId = input.Source.PackageId,
                    Version = input.Version.Version,
                },
            });

            return true;
        }
    }
}
