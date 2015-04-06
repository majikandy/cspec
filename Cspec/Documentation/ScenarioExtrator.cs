namespace Cspec.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Cspec.Framework;

    public class ScenarioExtrator : IExtractScenarios
    {
        private readonly IExtractGivenWhenThens givenWhenThensExtractor;

        public ScenarioExtrator(IExtractGivenWhenThens givenWhenThensExtractor)
        {
            this.givenWhenThensExtractor = givenWhenThensExtractor;
        }

        public IEnumerable<CriteriaInfo> GetImplementedCriteriaAndScenarios(MemberInfo featureType, IEnumerable<Type> allDerivationsOfFeature)
        {
            var derivationsOfFeature = allDerivationsOfFeature.ToList();
            var definedCriteria = this.GetAllCriteria(featureType);

            var implementedCriteria = this.GetAllImplementedCriteriaIncludingUnrequired(derivationsOfFeature);
            return implementedCriteria.Where(x => definedCriteria.Contains(x, new CompareCriteriaByFeatureName()));
        }

        public IEnumerable<string> GetPendingCriteria(MemberInfo featureType, IEnumerable<Type> allDerivationsOfFeature)
        {
            var allCriteria = this.GetAllCriteria(featureType);
            var implementedCriteria = this.GetImplementedCriteriaAndScenarios(featureType, allDerivationsOfFeature);

            return allCriteria.Except(implementedCriteria, new CompareCriteriaByFeatureName())
                .Select(x => x.Name);
        }

        public IEnumerable<CriteriaInfo> GetSupurfluousCriteria(MemberInfo featureType, IEnumerable<Type> allDerivationsOfFeature)
        {
            var allCriteria = this.GetAllCriteria(featureType);
            var implementedCriteria = this.GetAllImplementedCriteriaIncludingUnrequired(allDerivationsOfFeature);

            Console.WriteLine(allCriteria.Count());
            Console.WriteLine(implementedCriteria.Count());

            return implementedCriteria.Where(x => !allCriteria.Contains(x, new CompareCriteriaByFeatureName()));
        }

        private IEnumerable<CriteriaInfo> GetAllCriteria(MemberInfo featureType)
        {
            return featureType.GetCustomAttributes<Criteria>(true).Single()
                .Value.Select(x => new CriteriaInfo(x, null, null));
        }

        private IEnumerable<CriteriaInfo> GetAllImplementedCriteriaIncludingUnrequired(IEnumerable<Type> derivationsOfFeature)
        {
            return derivationsOfFeature.ToList()
                .SelectMany(t => t.GetMethods())
                .Where(m => Attribute.IsDefined(m, typeof(TestShowingAttribute)))
                .Select(theMethod => new { TheAttribute = this.GetTestShowingAttribute(theMethod), TheMethod = theMethod })
                .Select(
                    x =>
                    new CriteriaInfo(
                        name: x.TheAttribute.Functionality,
                        testMethodName: x.TheMethod.Name,
                        givenWhenThens: this.givenWhenThensExtractor.GetGivenWhenThens(x.TheMethod.Name, derivationsOfFeature)));
        }

        private TestShowingAttribute GetTestShowingAttribute(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes<TestShowingAttribute>(true).Single();
        }
    }
}