namespace Cspec.Generators
{
    using System.Collections.Generic;
    using System.Reflection;

    using Cspec.Extractor;

    public abstract class GeneratorBase : IGenerateFeatures
    {
        public abstract string Build(IEnumerable<FeatureInfo> features);

        public string Build(Assembly assemblyContainingFeatures)
        {
            return this.Build(
                new FeatureExtractor(
                    new FeatureDescriptionExtractor(),
                    new ScenarioExtrator(new GivenWhenThensExtractor(new FeatureFilePathProvider()))).ExtractFeatures(
                        assemblyContainingFeatures));
        }
    }
}