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
            this.WriteCriteria(writer, featureInfo.Scenarios.GroupBy(x => x.Name));

            this.WritePendingCriteria(writer, featureInfo, featureInfo.PendingScenarios);
            
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

        private void WriteCriteria(HtmlTextWriter writer, IEnumerable<IGrouping<string, Scenario>> criteria)
        {
            writer.OpenDiv("criteria text-success");

            writer.WriteTag(HtmlTextWriterTag.H4, "Current");

            foreach (var criterion in criteria)
            {
                WriteCriterion(writer, criterion);
            }

            writer.CloseTag();
        }

        private void WriteCriterion(HtmlTextWriter writer, IGrouping<string, Scenario> criterion)
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

        private void WriteTestMethodName(HtmlTextWriter writer, Scenario criterion)
        {
            writer.WriteTag(
                HtmlTextWriterTag.Div,
                "--> {0}".With(criterion.TestMethodName.WithSpacesInsteadOfUnderscores()),
                "test-method-name");
        }

        private void WriteGivenWhenThens(HtmlTextWriter writer, Scenario criterion)
        {
            writer.OpenDiv("givenWhenThens");

            foreach (var step in criterion.GivenWhenThens)
            {
                writer.WriteTag(HtmlTextWriterTag.Div, "----> {0}".With(step), "step");
            }

            writer.CloseTag();
        }

        private void WritePendingCriteria(HtmlTextWriter writer, FeatureInfo featureInfo, IEnumerable<string> pendingScenarios)
        {
            writer.OpenDiv("impending-criteria text-warning bg-warning");

            if (featureInfo.PendingScenarios.Any())
            {
                writer.WriteTag(HtmlTextWriterTag.H4, "Pending:");
            }

            foreach (var criteria in featureInfo.PendingScenarios)
            {
                writer.WriteTag(HtmlTextWriterTag.Div, criteria, "criteria");
            }

            writer.CloseTag();
        }
    }
}