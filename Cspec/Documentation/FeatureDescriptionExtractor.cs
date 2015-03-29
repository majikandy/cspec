namespace Cspec.Documentation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Cspec.Common;
    using Cspec.Framework;

    public class FeatureDescriptionExtractor : IExtractFeatureDescription
    {
        public IEnumerable<string> GetFeatureDescription(MemberInfo featureType)
        {
            var inOrderTo = "In order to {0}".FormatWith(this.GetAcceptanceAttribute<In_order_toAttribute>(featureType));
            var asA = "As a {0}".FormatWith(this.GetAcceptanceAttribute<As_aAttribute>(featureType));
            var want = "I want {0}".FormatWith(this.GetAcceptanceAttribute<I_wantAttribute>(featureType));
            return new List<string> { inOrderTo, asA, want };
        }

        private string GetAcceptanceAttribute<T>(MemberInfo featureType) where T : IAcceptanceAttribute
        {
            return ((IAcceptanceAttribute)featureType.GetCustomAttributes().Single(x => x.GetType() == typeof(T))).Text;
        }
    }
}