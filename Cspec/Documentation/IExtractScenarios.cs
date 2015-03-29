namespace Cspec.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IExtractScenarios
    {
        IEnumerable<Scenario> GetScenarios(IEnumerable<Type> allDerivationsOfFeature, Type featureType);
        IEnumerable<string> GetPendingScenarios(MemberInfo featureType);
    }
}