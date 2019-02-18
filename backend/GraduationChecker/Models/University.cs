using System.Linq;

namespace GraduationChecker.Models
{
    public class University
    {
        public string Id { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }

        public University() // Empty constructors for incoming HTTP payloads
        {
        }

        public University(TrackerMultiAsset trackerMultiAsset)
        {
            var universityNameCode = TrackerAssetCategoriesMap.Map[TrackerAssetCategories.UniversityName];

            Id = trackerMultiAsset.Id;
            LongName = trackerMultiAsset.Name;
            ShortName = trackerMultiAsset.Assets
                .Where(x => x.DisplayName.Split('|').Length > 1)
                .FirstOrDefault(x => x.DisplayName.Split('|')[1] == universityNameCode).DisplayName.Split('|')[0];
        }

        public TrackerCreateMultiAssetRequest ToMultiAssetRequest(System.Collections.Generic.IEnumerable<TrackerAsset> existingAssets)
        {
            var allAssetDisplayNames = new[]
                {
                    TrackerAssetCategoriesMap.Map[TrackerAssetCategories.UNIVERSITY],
                    $"{ShortName.Trim()}|{TrackerAssetCategoriesMap.Map[TrackerAssetCategories.UniversityName]}",
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
