namespace GraduationChecker.Models
{
    public class KeeperFile
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
    }

    public class KeeperVersion
    {
        public string Id { get; set; }
    }

    public class KeeperCompareResult
    {
        public decimal Accuracy { get; set; }
        public string FileUrl { get; set; }
    }

    public class KeeperResponse
    {
        public bool IsValid { get; set; }
        public string FileDiff { get; set; }
    }
}
