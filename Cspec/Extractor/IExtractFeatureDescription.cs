namespace Cspec.Extractor
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface IExtractFeatureDescription
    {
        IEnumerable<string> GetFeatureDescription(MemberInfo featureType);
    }
}