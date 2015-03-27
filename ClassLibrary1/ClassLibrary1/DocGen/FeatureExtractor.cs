namespace CSpec.DocGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using CSpec.Framework;

    public class FeatureExtractor
    {
        public IEnumerable<FeatureInfo> ExtractFeatures(Assembly assembly)
        {
            var features = new List<FeatureInfo>();
            var featureTypes = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(In_order_toAttribute), false).Any());
            
            foreach (var featureType in featureTypes)
            {
                Console.WriteLine("Found feature " + featureType.Name);

                var allDerivationsOfFeature = assembly.GetTypes().Where(x => featureType.IsAssignableFrom(x) && x != featureType);

                features.Add(new FeatureInfo()
                    {
                        Name = featureType.Name.Replace("Feature", string.Empty),
                        Scenarios = allDerivationsOfFeature
                                .SelectMany(t => t.GetMethods())
                                .Where(m => Attribute.IsDefined(m, typeof(TestShowingAttribute)))
                                .Select(theMethod => new { TheAttribute = GetTestShowingAttribute(theMethod), TheMethod = theMethod })
                                .Select(x => new Scenario(x.TheAttribute.Functionality, x.TheMethod.Name, new List<string>())),
                        PendingScenarios = this.GetPendingScenarios(featureType), 
                        AcceptanceDescription = this.GetFeatureDescription(featureType)
                    });
            }

            return features;
        }

        private static TestShowingAttribute GetTestShowingAttribute(MethodInfo x)
        {
            return (TestShowingAttribute)x.GetCustomAttributes(typeof(TestShowingAttribute), true).Single();
        }

        private IEnumerable<string> GetPendingScenarios(MemberInfo featureType)
        {
            var pending = featureType.GetCustomAttributes(typeof(PendingScenariosAttribute), true).SingleOrDefault();
            return pending != null ? pending.ToString().Split("\r\n".ToCharArray()).Select(x => x.Trim()).Where(y => !string.IsNullOrWhiteSpace(y)) : new List<string>();
        }

        private string GetAcceptanceAttribute<T>(MemberInfo featureType) where T : IAcceptanceAttribute
        {
            return ((IAcceptanceAttribute)featureType.GetCustomAttributes().Single(x => x.GetType() == typeof(T))).Text;
        }

        private IEnumerable<string> GetFeatureDescription(MemberInfo featureType)
        {
            var inOrderTo = "In order to {0}".FormatWith(this.GetAcceptanceAttribute<In_order_toAttribute>(featureType));
            var asA = "As a {0}".FormatWith(this.GetAcceptanceAttribute<As_aAttribute>(featureType));
            var want = "I want {0}".FormatWith(this.GetAcceptanceAttribute<I_wantAttribute>(featureType));
            return new List<string> { inOrderTo, asA, want };
        }
    }
}