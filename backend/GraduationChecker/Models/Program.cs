using System.Collections.Generic;
using System.Linq;

namespace GraduationChecker.Models
{
    public class Program
    {
        public string Id { get; set; }
        public string UniversityName { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string Degree { get; set; }
        public string Year { get; set; }

        public Program() // Empty constructors for incoming HTTP payloads
        {
        }

        public Program(TrackerMultiAsset trackerMultiAsset)
        {
            var trackerAssetCategoriesByString = TrackerAssetCategoriesMap.Map.ToDictionary(x => x.Value, x => x.Key);
            var propsByTrackerCategory = new Dictionary<TrackerAssetCategories, string>();
            foreach (var asset in trackerMultiAsset.Assets)
            {
                var data = asset.DisplayName.Split('|');
                if (data.Length > 1)
                {
                    propsByTrackerCategory.Add(trackerAssetCategoriesByString[data[1]], data[0]);
                }
            }

            Id = trackerMultiAsset.Id;
            LongName = trackerMultiAsset.Name;
            UniversityName = propsByTrackerCategory[TrackerAssetCategories.UniversityName];
            ShortName = propsByTrackerCategory[TrackerAssetCategories.ProgramName];
            Degree = propsByTrackerCategory[TrackerAssetCategories.ProgramDegree];
            Year = propsByTrackerCategory[TrackerAssetCategories.ProgramYear];
        }

        public TrackerCreateMultiAssetRequest ToMultiAssetRequest(IEnumerable<TrackerAsset> existingAssets)
        {
            var allAssetDisplayNames = new[]
                {
                    TrackerAssetCategoriesMap.Map[TrackerAssetCategories.PROGRAM],
                    $"{UniversityName}|{TrackerAssetCategoriesMap.Map[TrackerAssetCategories.UniversityName]}",
                    $"{ShortName}|{TrackerAssetCategoriesMap.Map[TrackerAssetCategories.ProgramName]}",
                    $"{Degree}|{TrackerAssetCategoriesMap.Map[TrackerAssetCategories.ProgramDegree]}",
                    $"{Year}|{TrackerAssetCategoriesMap.Map[TrackerAssetCategories.ProgramYear]}",
                };

            var existingAssetIdsByName = existingAssets.ToDictionary(x => x.Name.ToLower(), x => x.Id);
            var assetDisplayNames = allAssetDisplayNames.Where(x => !existingAssetIdsByName.ContainsKey(x.ToLower()));
            var assetIds = allAssetDisplayNames.Where(x => existingAssetIdsByName.ContainsKey(x.ToLower())).Select(x => existingAssetIdsByName[x.ToLower()]);

            return new TrackerCreateMultiAssetRequest
            {
                Name = LongName,
                AssetDisplayNames = assetDisplayNames,
                AssetIds = assetIds
            };
        }
    }
}
