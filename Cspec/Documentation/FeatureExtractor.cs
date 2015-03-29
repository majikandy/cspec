namespace Cspec.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Cspec.Framework;

    public class FeatureExtractor
    {
        private readonly IExtractFeatureDescription featureDescriptionExtractor;
        private readonly IExtractScenarios scenarioExtrator;

        public FeatureExtractor(
            IExtractFeatureDescription featureDescriptionExtractor, 
            IExtractScenarios scenarioExtractor)
        {
            this.featureDescriptionExtractor = featureDescriptionExtractor;
            this.scenarioExtrator = scenarioExtractor;
        }

        public IEnumerable<FeatureInfo> ExtractFeatures(Assembly assembly)
        {
            var features = new List<FeatureInfo>();
            var featureTypes = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(In_order_toAttribute), false).Any());
            
            foreach (var featureType in featureTypes)
            {
                var allDerivationsOfFeature = assembly.GetTypes().Where(x => featureType.IsAssignableFrom(x) && x != featureType);

                features.Add(new FeatureInfo()
                    {
                        Name = featureType.Name.Replace("Feature", string.Empty),
                        Scenarios = this.scenarioExtrator.GetScenarios(allDerivationsOfFeature, featureType),
                        PendingScenarios = this.scenarioExtrator.GetPendingScenarios(featureType), 
                        AcceptanceDescription = this.featureDescriptionExtractor.GetFeatureDescription(featureType)
                    });
            }

            return features;
        }
    }
}