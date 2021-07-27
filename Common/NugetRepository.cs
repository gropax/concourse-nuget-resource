using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NuGet;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System.Threading;

namespace Check
{
    public class NugetRepository
    {
        private string _repoUri;
        private SourceRepository _repo;
        public NugetRepository(string repoUri)
        {
            _repoUri = repoUri;
			_repo = GetSourceRepository(repoUri);
        }

		private SourceRepository GetSourceRepository(string repositoryUrl)
        {
			List<Lazy<INuGetResourceProvider>> providers = new List<Lazy<INuGetResourceProvider>>();
			providers.AddRange(Repository.Provider.GetCoreV3());
			PackageSource packageSource = new PackageSource(repositoryUrl);

			return new SourceRepository(packageSource, providers);
        }


		public async Task<string[]> GetNewPackageVersions(string packageId, string currentVersion = null)
        {
			string[] newVersions;

			using (var sourceCacheContext = new SourceCacheContext())
			{
				var findResource = await _repo.GetResourceAsync<FindPackageByIdResource>(CancellationToken.None);
				IEnumerable<NuGetVersion> versions = await findResource.GetAllVersionsAsync(packageId, sourceCacheContext, NullLogger.Instance, CancellationToken.None);

				var current = NuGetVersion.Parse(currentVersion ?? "0.0.0");
				newVersions = versions.Where(v => v >= current).Select(v => v.ToString()).ToArray();
			}

			return newVersions;
        }

        public async Task<string> DownloadPackage(string packageId, string version, string targetDir)
        {
            string packagePath = Path.Combine(targetDir, $"{packageId}.{version}.nupkg");
            PackageIdentity newestPackageIdentity = new PackageIdentity(packageId, NuGetVersion.Parse(version));

            using (var sourceCacheContext = new SourceCacheContext())
            {
                var packageDownloadContext = new PackageDownloadContext(sourceCacheContext, targetDir, true);
                using (DownloadResourceResult downloadResult = await NuGet.PackageManagement.PackageDownloader.GetDownloadResourceResultAsync(
                    new List<SourceRepository> { _repo }, newestPackageIdentity, packageDownloadContext, "",
                    NullLogger.Instance, CancellationToken.None))
                {
                    if (downloadResult.Status == DownloadResourceResultStatus.NotFound)
                        throw new Exception($".nupkg file not found for package {packageId} in repository {_repoUri}.");
                    if (downloadResult.Status == DownloadResourceResultStatus.Cancelled)
                        throw new Exception($"Download of .nupkg file was cancelled for package {packageId}. Please try again.");
                    if (downloadResult.Status == DownloadResourceResultStatus.AvailableWithoutStream)
                        throw new Exception($"Error retrieving stream of the downloaded .nupkg file for package {packageId}. Please try again.");

                    using (FileStream fileStream = File.Create(packagePath, (int)downloadResult.PackageStream.Length))
                    {
                        byte[] bytesInStream = new byte[downloadResult.PackageStream.Length];
                        downloadResult.PackageStream.Read(bytesInStream, 0, (int)downloadResult.PackageStream.Length);
                        fileStream.Write(bytesInStream, 0, (int)downloadResult.PackageStream.Length);
                    }
                }
            }

            return packagePath;
        }

        //public async static Task UploadPackage(string nugetRepositoryUrl, string packagePath, string apiKey)
        //{
        //	List<Lazy<INuGetResourceProvider>> providers = new List<Lazy<INuGetResourceProvider>>();
        //	providers.AddRange(Repository.Provider.GetCoreV3());
        //	PackageSource packageSource = new PackageSource(nugetRepositoryUrl);
        //	SourceRepository sourceRepository = new SourceRepository(packageSource, providers);

        //	using (var sourceCacheContext = new SourceCacheContext())
        //	{
        //		PackageUpdateResource uploadResource = await sourceRepository.GetResourceAsync<PackageUpdateResource>();
        //		await uploadResource.Push(packagePath, null, 480, false, (param) => apiKey, null, NullLogger.Instance);
        //	}
        //}


    }
}
