namespace Cspec.Generators 
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.UI;

    using Cspec.Common;
    using Cspec.Extractor;

    public class HtmlFeatureGenerator : GeneratorBase
    {
        private int criterionCollapserCounter = 1;

        public override string Build(IEnumerable<FeatureInfo> features)
        {
            var stringWriter = new StringWriter();
            var writer = new HtmlTextWriter(stringWriter);

            writer.OpenDiv("container");
            writer.WriteStylesheet(@"Content\bootstrap.min.css");
            writer.WriteScript(@"Scripts\jquery-1.9.1.min.js");
            writer.WriteScript(@"Scripts\bootstrap.min.js");
            
            this.WriteFeatures(writer, features);

            writer.CloseTag();

            return stringWriter.ToString().TrimWhitespaceWithinHtml();
        }

        private void WriteFeatures(HtmlTextWriter writer, IEnumerable<FeatureInfo> features)
        {
            writer.OpenDiv("features");

            foreach (var feature in features)
            {
                this.WriteFeature(writer, feature);
            }

            writer.CloseTag();
        }

        private void WriteFeature(HtmlTextWriter writer, FeatureInfo featureInfo)
        {
            var featureId = featureInfo.Id;
            var featureCollapserName = featureId + "Collapser";

            writer.OpenDiv("feature well");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "h2");
                writer.AddAttribute("data-toggle", "collapse");
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#" + featureCollapserName);
                writer.AddAttribute("aria-expanded", "false");
                writer.AddAttribute("aria-controls", featureCollapserName);
                writer.WriteTag(HtmlTextWriterTag.A, featureInfo.Name);

                writer.AddAttribute("id", featureCollapserName);
                writer.OpenDiv("collapse");
                    writer.WriteBreak();
                    this.WriteFeatureDesciption(featureInfo, writer);
                    this.WriteCriteria(writer, featureInfo.Criteria.GroupBy(x => x.Name));

                    this.WritePendingCriteria(writer, featureInfo.PendingCriteria);
                    this.WriteSuperfluousCriteria(writer, featureInfo.SuperfluousCriteria);
                writer.CloseTag();

            writer.CloseTag();
        }

        private void WriteFeatureDesciption(FeatureInfo featureInfo, HtmlTextWriter writer)
        {
            writer.OpenDiv("description well");

            foreach (var line in featureInfo.AcceptanceDescription)
            {
                writer.WriteTag(HtmlTextWriterTag.Div, line.Trim());
            }

            writer.RenderEndTag();
        }

        private void WriteCriteria(HtmlTextWriter writer, IEnumerable<IGrouping<string, CriteriaInfo>> criteria)
        {
            writer.OpenDiv("criteria text-success");

            writer.WriteTag(HtmlTextWriterTag.H4, "Current");

            foreach (var criterion in criteria)
            {
                this.WriteCriterion(writer, criterion);
            }

            writer.CloseTag();
        }

        private void WriteCriterion(HtmlTextWriter writer, IGrouping<string, CriteriaInfo> criterion)
        {
            WriteCriterionDescription(writer, criterion);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, "criterion-collapser-" + criterionCollapserCounter++);
            writer.OpenDiv("criterion");

            foreach (var scenario in criterion)
            {
                writer.OpenDiv("scenario");
                this.WriteTestMethodName(writer, scenario);
                this.WriteGivenWhenThens(writer, scenario);

                writer.RenderEndTag();
            }

            writer.CloseTag();
        }

        private void WriteCriterionDescription(HtmlTextWriter writer, IGrouping<string, CriteriaInfo> criterion)
        {
            writer.OpenTag(HtmlTextWriterTag.Strong, string.Empty);
            writer.OpenTag(HtmlTextWriterTag.Div, string.Empty, "criterion-description");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#criterion-collapser-" + criterionCollapserCounter);
            writer.WriteTag(HtmlTextWriterTag.A, "-{0}".With(criterion.Key));
            writer.CloseTag();
            writer.CloseTag();
        }

        private void WriteTestMethodName(HtmlTextWriter writer, CriteriaInfo criterionInfo)
        {
            writer.WriteTag(
                HtmlTextWriterTag.Div,
                "--> {0}".With(criterionInfo.TestMethodName.WithSpacesInsteadOfUnderscores()),
                "test-method-name");
        }

        private void WriteGivenWhenThens(HtmlTextWriter writer, CriteriaInfo criterionInfo)
        {
            writer.OpenDiv("givenWhenThens");

            foreach (var step in criterionInfo.GivenWhenThens)
            {
                writer.WriteTag(HtmlTextWriterTag.Div, "----> {0}".With(step), "step");
            }

            writer.CloseTag();
        }

        private void WritePendingCriteria(HtmlTextWriter writer, IEnumerable<string> pendingScenarios)
        {
            writer.OpenDiv("pending-criteria text-warning bg-warning");

            if (pendingScenarios.Any())
            {
                writer.WriteTag(HtmlTextWriterTag.H4, "Pending:");
            }

            foreach (var criteria in pendingScenarios)
            {
                writer.WriteTag(HtmlTextWriterTag.Div, criteria, "criteria");
            }

            writer.CloseTag();
        }

        private void WriteSuperfluousCriteria(HtmlTextWriter writer, IEnumerable<CriteriaInfo> superfluousCriteria)
        {
            writer.OpenDiv("superfluous-criteria text-warning bg-error");

            if (superfluousCriteria.Any())
            {
                writer.WriteTag(HtmlTextWriterTag.H4, "Superfluous:");
            }

            foreach (var criteria in superfluousCriteria)
            {
                writer.WriteTag(HtmlTextWriterTag.Div, criteria.Name, "criteria");
            }

            writer.CloseTag();
        }
    }
}