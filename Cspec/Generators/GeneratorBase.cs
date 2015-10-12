namespace Cspec.Generators
{
    using System.Collections.Generic;
    using System.Reflection;

    using Cspec.Extractor;

    public abstract class GeneratorBase : IGenerateFeatures
    {
        private const string DefaultFeaturesFolder = @"..\..\Features";

        public abstract string Build(IEnumerable<FeatureInfo> features);

        public string Build(Assembly assemblyContainingFeatures, string featuresFolderPath = null)
        {
            if (featuresFolderPath == null)
            {
                featuresFolderPath = DefaultFeaturesFolder;
            }

            return this.Build(
                new FeatureExtractor(
                    new FeatureDescriptionExtractor(),
                    new ScenarioExtrator(new GivenWhenThensExtractor(featuresFolderPath)))
                        .ExtractFeatures(assemblyContainingFeatures)
                );
        }
    }
}