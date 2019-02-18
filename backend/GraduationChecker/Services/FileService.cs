using GraduationChecker.Config;
using GraduationChecker.Models;
using GraduationChecker.RestClients;
using Mantle.Core.Lib;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GraduationChecker.Services
{
    public class FileService
    {
        private readonly MantleRestClient _mantleRestClient;
        private readonly string _keeperProductId;
        private readonly string _keeperFolderId;

        public FileService(MantleRestClient mantleRestClient, AppSettings appSettings)
        {
            _mantleRestClient = mantleRestClient;
            _keeperProductId = appSettings.MantleKeeperProductId;
            _keeperFolderId = appSettings.MantleKeeperFolderId;
        }

        public async Task<IEnumerable<KeeperFile>> GetFiles()
        {
            return await _mantleRestClient.SendRequestAsync<IEnumerable<KeeperFile>>($"/keeper/{_keeperProductId}/files?folderId={_keeperFolderId}&limit=1000", HttpMethod.Get);
        }

        public async Task<IEnumerable<KeeperVersion>> GetVersions(string fileId)
        {
            return await _mantleRestClient.SendRequestAsync<IEnumerable<KeeperVersion>>($"/keeper/{_keeperProductId}/files/{fileId}/versions", HttpMethod.Get);
        }

        public async Task<KeeperCompareResult> CompareVersion(string fileId, string versionId, IFormFile file)
        {
            var request = new RestRequestConfig { Body = new { File = file, MustGenerateCompareResult = true }, ContentType = ContentType.MultipartFormData };
            var path = versionId == fileId ? $"/keeper/{_keeperProductId}/files/{fileId}/compare/original" : $"/keeper/{_keeperProductId}/files/{fileId}/versions/compare/{versionId}";
            return await _mantleRestClient.SendRequestAsync<KeeperCompareResult>(path, HttpMethod.Post, request);
        }
    }
}
