namespace Cspec.Generators
{
    using System.Collections.Generic;
    using System.Reflection;

    using Cspec.Extractor;

    public interface IGenerateFeatures
    {
        string Build(IEnumerable<FeatureInfo> features);

        string Build(Assembly assemblyContainingFeatures, string featuresFolderPath = null);
    }
}