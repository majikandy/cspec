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

        public IEnumerable<Scenario> GetScenarios(IEnumerable<Type> allDerivationsOfFeature, Type featureType)
        {
            return allDerivationsOfFeature
                .SelectMany(t => t.GetMethods())
                .Where(m => Attribute.IsDefined(m, typeof(TestShowingAttribute)))
                .Select(theMethod => new { TheAttribute = this.GetTestShowingAttribute(theMethod), TheMethod = theMethod })
                .Select(x => 
                        new Scenario(
                            name: x.TheAttribute.Functionality, 
                            testMethodName: x.TheMethod.Name, 
                            givenWhenThens: this.givenWhenThensExtractor.GetGivenWhenThens(x.TheMethod.Name, featureType.Name)));
        }

        private TestShowingAttribute GetTestShowingAttribute(MethodInfo x)
        {
            return (TestShowingAttribute)x.GetCustomAttributes(typeof(TestShowingAttribute), true).Single();
        }

        public IEnumerable<string> GetPendingScenarios(MemberInfo featureType)
        {
            var pending = featureType.GetCustomAttributes(typeof(PendingScenariosAttribute), true)
                .SingleOrDefault();
            return pending != null ? pending.ToString().Split("\r\n".ToCharArray()).Select(x => x.Trim()).Where(y => !string.IsNullOrWhiteSpace(y)) : new List<string>();
        }
    }
}