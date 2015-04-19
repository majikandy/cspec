namespace Cspec.Generators
{
    using System.Collections.Generic;

    using Cspec.Extractor;

    public interface IGenerateFeatures
    {
        string Build(IEnumerable<FeatureInfo> features);

        string Build();
    }
}