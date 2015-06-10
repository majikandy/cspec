namespace Cspec.Extractor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Cspec.Common;
    using Cspec.Framework;

    using NUnit.Framework;

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

        public IEnumerable<CriteriaInfo> GetSuperfluousCriteria(MemberInfo featureType, IEnumerable<Type> allDerivationsOfFeature)
        {
            var allCriteria = this.GetAllCriteria(featureType);
            var implementedCriteria = this.GetAllImplementedCriteriaIncludingUnrequired(allDerivationsOfFeature);

            return implementedCriteria.Where(x => !allCriteria.Contains(x, new CompareCriteriaByFeatureName()));
        }

        private IEnumerable<CriteriaInfo> GetAllCriteria(MemberInfo featureType)
        {
            var criteria = featureType.GetCustomAttributes<Criteria>(true).SingleOrDefault();
            if (criteria == null)
            {
                throw new CriteriaNotSpecifiedException(@"Expected '{0}' to have Criteria Attribute eg  [Criteria(new[] {{ ""should show message when there is an error"", ""should not show any messages when succeeds"" }})]".With(featureType.Name));
            }
            
            return criteria.Value.Select(x => new CriteriaInfo(x, null, null));
        }

        private IEnumerable<CriteriaInfo> GetAllImplementedCriteriaIncludingUnrequired(IEnumerable<Type> derivationsOfFeature)
        {
            return derivationsOfFeature.ToList()
                .SelectMany(t => t.GetMethods())
                .Where(m => Attribute.IsDefined(m, typeof(TestAttribute)))
                .Where(m => !Attribute.IsDefined(m, typeof(IgnoreAttribute)))
                .Select(theMethod => new { TheAttribute = this.GetTestAttribute(theMethod), TheMethod = theMethod, TheClassItIsIn = theMethod.DeclaringType.Name })
                .Select(
                    x =>
                    new CriteriaInfo(
                        name: x.TheAttribute.Description ?? x.TheClassItIsIn.TrasformCamelOrSnakeToEnglish(),
                        testMethodName: x.TheMethod.Name,
                        givenWhenThens: this.givenWhenThensExtractor.GetGivenWhenThens(x.TheMethod.Name, derivationsOfFeature)));
        }

        

        private TestAttribute GetTestAttribute(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes<TestAttribute>(true).Single();
        }
    }
}