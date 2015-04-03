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

            writer.OpenTag(HtmlTextWriterTag.Div, string.Empty, "container");

            writer.WriteStylesheet("bootstrap.min.css");
            WriteFeatures(writer, features);

            writer.CloseTag();

            return stringWriter.ToString().TrimWhitespaceWithinHtml();
        }

        private void WriteFeatures(HtmlTextWriter writer, IEnumerable<FeatureInfo> features)
        {
            writer.OpenTag(HtmlTextWriterTag.Div, string.Empty, "features");

            foreach (var feature in features)
            {
                this.WriteFeatureDiv(writer, feature);
            }

            writer.CloseTag();
        }

        private void WriteFeatureDiv(HtmlTextWriter writer, FeatureInfo featureInfo)
        {
            writer.OpenTag(HtmlTextWriterTag.Div, string.Empty, "feature well");

            writer.WriteTag(HtmlTextWriterTag.H2, featureInfo.Name.Replace("Feature", string.Empty));

            this.BuildFeatureDesciption(featureInfo, writer);
            this.BuildCriteriaDiv(writer, featureInfo.Scenarios.GroupBy(x => x.Name));

            this.WritePendingCriteria(writer, featureInfo, featureInfo.PendingScenarios);
            
            writer.CloseTag();
        }

        private void BuildFeatureDesciption(FeatureInfo featureInfo, HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "description well");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            foreach (var line in featureInfo.AcceptanceDescription)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write(line.Trim());
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }

        private void BuildCriteriaDiv(HtmlTextWriter writer, IEnumerable<IGrouping<string, Scenario>> criteria)
        {
            writer.OpenTag(HtmlTextWriterTag.Div, string.Empty, "criteria text-success");
            writer.WriteTag(HtmlTextWriterTag.H4, "Current");

            foreach (var criterion in criteria)
            {
                WriteCriterion(writer, criterion);
            }

            writer.CloseTag();
        }

        private void WriteCriterion(HtmlTextWriter writer, IGrouping<string, Scenario> criterion)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "criterion");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.WriteTag(HtmlTextWriterTag.Div, "-{0}".With(criterion.Key), "criterion-description");

            foreach (var scenario in criterion)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "scenario");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                WriteTestMethodName(writer, scenario);
                WriteGivenWhenThens(writer, scenario);

                writer.RenderEndTag();
            }
            writer.RenderEndTag();
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
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "givenWhenThens");

            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            foreach (var step in criterion.GivenWhenThens)
            {
                writer.WriteTag(HtmlTextWriterTag.Div, "----> {0}".With(step), "step");
            }

            writer.RenderEndTag();
        }

        private void WritePendingCriteria(HtmlTextWriter writer, FeatureInfo featureInfo, IEnumerable<string> pendingScenarios)
        {
            writer.OpenTag(HtmlTextWriterTag.Div, string.Empty, "impending-criteria text-warning bg-warning");

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