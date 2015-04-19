namespace Cspec.Extractor
{
    using System.Collections.Generic;

    public class CompareCriteriaByFeatureName : IEqualityComparer<CriteriaInfo>
    {
        public bool Equals(CriteriaInfo x, CriteriaInfo y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(CriteriaInfo obj)
        {
            return 0;
        }
    }
}