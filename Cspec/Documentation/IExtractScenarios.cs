namespace Cspec.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IExtractScenarios
    {
        IEnumerable<CriteriaInfo> GetImplementedCriteriaAndScenarios(MemberInfo featureType, IEnumerable<Type> allDerivationsOfFeature);
        IEnumerable<string> GetPendingCriteria(MemberInfo featureType, IEnumerable<Type> allDerivationsOfFeature);
        IEnumerable<CriteriaInfo> GetSupurfluousCriteria(MemberInfo featureType, IEnumerable<Type> allDerivationsOfFeature);
    }
}