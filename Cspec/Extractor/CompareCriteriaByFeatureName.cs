namespace Cspec.Extractor
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class CompareCriteriaByFeatureName : IEqualityComparer<CriteriaInfo>
    {
        public bool Equals(CriteriaInfo x, CriteriaInfo y)
        {
            return RemovePunctuationAndLowerCase(x) == RemovePunctuationAndLowerCase(y);
        }

        private static string RemovePunctuationAndLowerCase(CriteriaInfo criteriaInfo)
        {
            var regex = new Regex("[^a-z0-9]");
            return regex.Replace(criteriaInfo.Name.ToLower(), string.Empty);
        }

        public int GetHashCode(CriteriaInfo obj)
        {
            return 0;
        }
    }
}