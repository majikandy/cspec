namespace Cspec.Documentation
{
    using System.Collections.Generic;

    public class FeatureInfo
    {
        public string Name { get; set; }
        public IEnumerable<string> AcceptanceDescription { get; set; }
        public IEnumerable<CriteriaInfo> Criteria { get; set; }
        public IEnumerable<string> PendingCriteria { get; set; }
        public IEnumerable<CriteriaInfo> SupurfluousCriteria { get; set; }
    }
}