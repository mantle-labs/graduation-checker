using System.Collections.Generic;

namespace GraduationChecker.Models
{
    public class TrackerAsset
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class TrackerMultiAsset : TrackerAsset
    {
        public IEnumerable<SubAssetIdentity> Assets { get; set; }
    }

    public class SubAssetIdentity
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
    }

    public class TrackerCreateAssetRequest
    {
        public string Name { get; set; }
    }

    public class TrackerCreateMultiAssetRequest : TrackerCreateAssetRequest
    {
        public IEnumerable<string> AssetIds { get; set; }
        public IEnumerable<string> AssetDisplayNames { get; set; }
    }

    public enum TrackerAssetCategories
    {
        PROGRAM = 0,
        UNIVERSITY = 1,
        UniversityName = 2,
        ProgramName = 3,
        ProgramDegree = 4,
        ProgramYear = 5
    }

    public static class TrackerAssetCategoriesMap
    {
        public static readonly Dictionary<TrackerAssetCategories, string> Map = new Dictionary<TrackerAssetCategories, string>
        {
            { TrackerAssetCategories.PROGRAM, "PRG" },
            { TrackerAssetCategories.UNIVERSITY, "UNI" },
            { TrackerAssetCategories.UniversityName, "UN" },
            { TrackerAssetCategories.ProgramName, "PN" },
            { TrackerAssetCategories.ProgramDegree, "PD" },
            { TrackerAssetCategories.ProgramYear, "PY" },
        };
    }
}
