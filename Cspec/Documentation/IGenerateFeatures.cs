namespace Cspec.Documentation
{
    using System.Collections.Generic;

    public interface IGenerateFeatures
    {
        string BuildFeatureDocumentation(IEnumerable<FeatureInfo> features);
    }
}