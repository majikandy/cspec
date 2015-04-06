namespace Cspec.Documentation 
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.UI;

    using Cspec.Common;

    public class HtmlFeatureGenerator : IGenerateFeatures
    {
        public string BuildFeatureDocumentation(IEnumerable<FeatureInfo> features)
        {
            var stringWriter = new StringWriter();
            var writer = new HtmlTextWriter(stringWriter);

            writer.OpenDiv("container");
            writer.WriteStylesheet("bootstrap.min.css");
            
            WriteFeatures(writer, features);

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
            writer.OpenDiv("feature well");

            writer.WriteTag(HtmlTextWriterTag.H2, featureInfo.Name.Replace("Feature", string.Empty));

            this.WriteFeatureDesciption(featureInfo, writer);
            this.WriteCriteria(writer, featureInfo.Criteria.GroupBy(x => x.Name));

            this.WritePendingCriteria(writer, featureInfo.PendingCriteria);
            this.WriteSupurfluousCriteria(writer, featureInfo.SupurfluousCriteria);
            
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
                WriteCriterion(writer, criterion);
            }

            writer.CloseTag();
        }

        private void WriteCriterion(HtmlTextWriter writer, IGrouping<string, CriteriaInfo> criterion)
        {
            writer.OpenDiv("criterion");

            writer.WriteTag(HtmlTextWriterTag.Div, "-{0}".With(criterion.Key), "criterion-description");

            foreach (var scenario in criterion)
            {
                writer.OpenDiv("scenario");

                WriteTestMethodName(writer, scenario);
                WriteGivenWhenThens(writer, scenario);

                writer.RenderEndTag();
            }

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

        private void WriteSupurfluousCriteria(HtmlTextWriter writer, IEnumerable<CriteriaInfo> supurfluousCriteria)
        {
            writer.OpenDiv("supurfluous-criteria text-warning bg-error");

            if (supurfluousCriteria.Any())
            {
                writer.WriteTag(HtmlTextWriterTag.H4, "Supurfluous:");
            }

            foreach (var criteria in supurfluousCriteria)
            {
                writer.WriteTag(HtmlTextWriterTag.Div, criteria.Name, "criteria");
            }

            writer.CloseTag();
        }
    }
}