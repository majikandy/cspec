namespace Cspec.Documentation
{
    using System.Collections.Generic;

    public class CompareCriteriaByFeatureName : IEqualityComparer<CriteriaInfo>
    {
        public bool Equals(CriteriaInfo x, CriteriaInfo y)
        {
            return x.Name.Trim() == y.Name.Trim();
        }

        public int GetHashCode(CriteriaInfo obj)
        {
            return 0;
        }
    }
}