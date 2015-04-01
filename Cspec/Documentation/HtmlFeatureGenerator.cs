namespace Cspec.Documentation
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.UI;

    using HtmlAgilityPack;

    public class HtmlFeatureGenerator : IGenerateFeatures
    {
        private HtmlNode br;
        private HtmlDocument htmlDoc;

        public string BuildFeatureDocumentation(IEnumerable<FeatureInfo> features)
        {
            this.htmlDoc = new HtmlDocument();
            var stringWriter = new StringWriter();
            var writer = new HtmlTextWriter(stringWriter);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "container");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "feature well");

            //this.br = this.htmlDoc.CreateElement("br");
            //var bootstrapCss = HtmlNode.CreateNode(@"<link rel='stylesheet' type='text/css' href='bootstrap.min.css' />");
            //var featuresDiv = this.htmlDoc.CreateElementWithAttribute("div", "class", "features");

            foreach (var feature in features)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "feature");
                this.BuildFeatureDiv(feature, writer);
            }

            var body = this.htmlDoc.CreateElementWithAttribute("div", "class", "container");
            //body.AppendChild(bootstrapCss);
            //body.AppendChild(featuresDiv);
            return stringWriter.ToString();
            return body.OuterHtml;
        }

        private void BuildFeatureDiv(FeatureInfo featureInfo, HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "feature well");
            writer.RenderBeginTag(HtmlTextWriterTag.H2);
            writer.Write(featureInfo.Name.Replace("Feature", string.Empty));
            writer.RenderEndTag();

            //featureDiv.AppendChild(this.BuildFeatureDesciption(featureInfo));
            //featureDiv.AppendChild(this.BuildCriteriaDiv(featureInfo));
            //featureDiv.AppendChild(this.BuildImpendingCriteria(featureInfo));
            //return featureDiv;
        }

        private HtmlNode BuildFeatureDesciption(FeatureInfo featureInfo)
        {
            var descriptionDiv = this.htmlDoc.CreateElementWithAttribute("div", "class", "description well");

            featureInfo.AcceptanceDescription
                .Select(x => this.htmlDoc.CreateElementWithText("div", x))
                .ToList().ForEach(d => descriptionDiv.AppendChild(d));

            return descriptionDiv;
        }

        private HtmlNode BuildCriteriaDiv(FeatureInfo featureInfo)
        {
            var featuringDiv = this.htmlDoc.CreateElementWithAttribute("div", "class", "criteria text-success");
            featuringDiv.AppendChild(this.htmlDoc.CreateElementWithText("h4", "Current"));

            foreach (var criteria in featureInfo.Scenarios.GroupBy(x => x.Name))
            {
                var criteriaSpan = this.htmlDoc.CreateElementWithAttributeAndText("div", "class", "criteria", "-" + criteria.Key);
                this.AppendChildListToNode(featuringDiv, criteriaSpan);
                foreach (var criterion in criteria)
                {
                    var criteriaTestMethodSpan = this.htmlDoc.CreateElementWithAttributeAndText("div", "class", "test-method", "-->" + criterion.TestMethodName.Replace("_", " "));
                    var givenWhenThensDiv = this.htmlDoc.CreateElementWithAttribute("div", "class", "givenWhenThens");
                    foreach (var step in criterion.GivenWhenThens)
                    {
                        var stepDiv = this.htmlDoc.CreateElementWithAttributeAndText("div", "class", "step", "---->" + step);
                        givenWhenThensDiv.AppendChild(stepDiv);
                    }
                    this.AppendChildListToNode(featuringDiv, criteriaTestMethodSpan);
                    featuringDiv.AppendChild(givenWhenThensDiv);
                }
            }

            return featuringDiv;
        }

        private HtmlNode BuildImpendingCriteria(FeatureInfo featureInfo)
        {
            var impendingCriteriaDiv = this.htmlDoc.CreateElementWithAttribute("div", "class", "impending-criteria text-warning bg-warning");

            if (featureInfo.PendingScenarios.Any())
            {
                impendingCriteriaDiv.AppendChild(this.htmlDoc.CreateElementWithText("h4", "Pending:"));
            }

            foreach (var criteria in featureInfo.PendingScenarios)
            {
                var criteriaSpan = this.htmlDoc.CreateElementWithAttributeAndText("span", "class", "criteria", criteria);
                this.AppendChildListToNode(impendingCriteriaDiv, criteriaSpan, this.br);
            }

            return impendingCriteriaDiv;
        }

        private void AppendChildListToNode(HtmlNode node, params HtmlNode[] children)
        {
            foreach (var child in children)
            {
                node.AppendChild(child);
            }
        }
    }
}