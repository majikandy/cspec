namespace Cspec.Extractor
{
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

        public FeatureExtractor() : this (new FeatureDescriptionExtractor(), new ScenarioExtrator(new GivenWhenThensExtractor(new FeatureFilePathProvider())))
        {
        }

        public IEnumerable<FeatureInfo> ExtractFeatures(Assembly assembly)
        {
            var features = new List<FeatureInfo>();
            var featureTypes = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(In_order_toAttribute), false).Any());
            
            foreach (var featureType in featureTypes)
            {
                var allDerivationsOfFeature = assembly.GetTypes().Where(x => featureType.IsAssignableFrom(x) && x != featureType).ToList();

                features.Add(new FeatureInfo()
                    {
                        Name = featureType.Name.Replace("Feature", string.Empty),
                        Criteria = this.scenarioExtrator.GetImplementedCriteriaAndScenarios(featureType, allDerivationsOfFeature),
                        PendingCriteria = this.scenarioExtrator.GetPendingCriteria(featureType, allDerivationsOfFeature), 
                        AcceptanceDescription = this.featureDescriptionExtractor.GetFeatureDescription(featureType),
                        SuperfluousCriteria = this.scenarioExtrator.GetSuperfluousCriteria(featureType, allDerivationsOfFeature)
                    });
            }

            return features;
        }
    }
}