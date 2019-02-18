using GraduationChecker.Config;
using GraduationChecker.Models;
using GraduationChecker.RestClients;
using Mantle.Core.Lib;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GraduationChecker.Services
{
    public class DataService
    {
        private readonly MantleRestClient _mantleRestClient;
        private readonly string _trackerProductId;

        public DataService(MantleRestClient mantleRestClient, AppSettings appSettings)
        {
            _mantleRestClient = mantleRestClient;
            _trackerProductId = appSettings.MantleTrackerProductId;
        }

        public async Task<University> GetUniversityById(string id)
        {
            var multiAsset = await _mantleRestClient.SendRequestAsync<TrackerMultiAsset>($"/tracker/{_trackerProductId}/multiassets/{id}", HttpMethod.Get);

            return new University(multiAsset);
        }

        public async Task<Models.Program> GetProgramById(string id)
        {
            var multiAsset = await _mantleRestClient.SendRequestAsync<TrackerMultiAsset>($"/tracker/{_trackerProductId}/multiassets/{id}", HttpMethod.Get);

            return new Models.Program(multiAsset);
        }

        public async Task<Dictionary<string, List<string>>> GetProgramSpecs()
        {
            var assets = await _mantleRestClient.SendRequestAsync<IEnumerable<TrackerAsset>>($"/tracker/{_trackerProductId}/assets?limit=1000", HttpMethod.Get);
            var trackerAssetCategoriesByString = TrackerAssetCategoriesMap.Map.ToDictionary(x => x.Value, x => x.Key);
            var propsByTrackerCategory = new Dictionary<string, List<string>>();

            foreach (var asset in assets)
            {
                var data = asset.Name.Split('|');
                if (data.Length > 1 && trackerAssetCategoriesByString.ContainsKey(data[1]))
                {
                    if (!propsByTrackerCategory.ContainsKey(data[1]))
                    {
                        propsByTrackerCategory[data[1]] = new List<string> { data[0] };
                    }
                    else
                    {
                        propsByTrackerCategory[data[1]].Add(data[0]);
                    }
                }
            }

            return propsByTrackerCategory;
        }

        public async Task<IEnumerable<University>> GetUniversities()
        {
            var universityAssetName = TrackerAssetCategoriesMap.Map[TrackerAssetCategories.UNIVERSITY];
            var universityRequest = new RestRequestConfig { Body = new { AssetDisplayNames = universityAssetName } };
            var universityAsset = (await _mantleRestClient.SendRequestAsync<IEnumerable<TrackerAsset>>($"/tracker/{_trackerProductId}/assets?limit=1000", HttpMethod.Get, universityRequest)).FirstOrDefault();

            if (universityAsset == null)
            {
                return Enumerable.Empty<University>();
            }

            var multiAssetRequest = new RestRequestConfig { Body = new { AssetIds = universityAsset.Id } };
            var multiAssets = await _mantleRestClient.SendRequestAsync<IEnumerable<TrackerMultiAsset>>($"/tracker/{_trackerProductId}/multiassets?limit=1000", HttpMethod.Get, multiAssetRequest);

            return multiAssets.Select(x => new University(x));
        }

        public async Task<IEnumerable<Models.Program>> GetPrograms(string universityName, string degree, string year)
        {
            var programAssetName = TrackerAssetCategoriesMap.Map[TrackerAssetCategories.PROGRAM];
            var programRequest = new RestRequestConfig { Body = new { AssetDisplayNames = programAssetName } };
            var programAsset = (await _mantleRestClient.SendRequestAsync<IEnumerable<TrackerAsset>>($"/tracker/{_trackerProductId}/assets?limit=1000", HttpMethod.Get, programRequest)).FirstOrDefault();

            if (programAsset == null)
            {
                return Enumerable.Empty<Models.Program>();
            }

            var multiAssetRequest = new RestRequestConfig { Body = new { AssetIds = programAsset.Id } };
            var multiAssets = await _mantleRestClient.SendRequestAsync<IEnumerable<TrackerMultiAsset>>($"/tracker/{_trackerProductId}/multiassets?limit=1000", HttpMethod.Get, multiAssetRequest);

            return multiAssets.Select(x => new Models.Program(x))
                .Where(x => (string.IsNullOrEmpty(universityName) || x.UniversityName == universityName)
                            && (string.IsNullOrEmpty(degree) || x.Degree == degree)
                            && (string.IsNullOrEmpty(year) || x.Year == year)); // Not efficient, but easy to code :)
        }

        public async Task CreateProgram(Models.Program program)
        {
            var trackerAssets = await _mantleRestClient.SendRequestAsync<IEnumerable<TrackerAsset>>($"/tracker/{_trackerProductId}/assets?limit=1000", HttpMethod.Get);

            var multiAssetRequest = new RestRequestConfig { Body = program.ToMultiAssetRequest(trackerAssets) };
            await _mantleRestClient.SendRequestAsync<string>($"/tracker/{_trackerProductId}/multiassets", HttpMethod.Post, multiAssetRequest);
        }

        public async Task CreateUniversity(University university)
        {
            var trackerAssets = await _mantleRestClient.SendRequestAsync<IEnumerable<TrackerAsset>>($"/tracker/{_trackerProductId}/assets?limit=1000", HttpMethod.Get);

            var multiAssetRequest = new RestRequestConfig { Body = university.ToMultiAssetRequest(trackerAssets) };
            await _mantleRestClient.SendRequestAsync<string>($"/tracker/{_trackerProductId}/multiassets", HttpMethod.Post, multiAssetRequest);
        }
    }
}
