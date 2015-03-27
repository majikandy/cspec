namespace CSpec.DocGen
{
    using System.Collections.Generic;
    using System.Linq;

    using HtmlAgilityPack;

    public class FeatureHtmlGenerator
    {
        private HtmlNode br;
        private HtmlDocument htmlDoc;

        public string BuildFeatureDocumentationHtml(IEnumerable<FeatureInfo> featureSets)
        {
            this.htmlDoc = new HtmlDocument();

            this.br = this.htmlDoc.CreateElement("br");
            var bootstrapCss = HtmlNode.CreateNode(@"<link rel='stylesheet' type='text/css' href='bootstrap.min.css' />");
            var featuresDiv = this.htmlDoc.CreateElementWithAttribute("div", "class", "features");

            foreach (var feature in featureSets)
            {
                featuresDiv.AppendChild(this.BuildFeatureDiv(feature));
            }

            var body = this.htmlDoc.CreateElementWithAttribute("div", "class", "container");
            body.AppendChild(bootstrapCss);
            body.AppendChild(featuresDiv);
            return body.OuterHtml;
        }

        private HtmlNode BuildFeatureDiv(FeatureInfo featureInfo)
        {
            var featureDiv = this.htmlDoc.CreateElementWithAttribute("div", "class", "feature well");
            var featureName = this.htmlDoc.CreateElementWithText("h2", featureInfo.Name.Replace("Feature", string.Empty));

            featureDiv.AppendChild(featureName);
            featureDiv.AppendChild(this.BuildFeatureDesciption(featureInfo));
            featureDiv.AppendChild(this.BuildCriteriaDiv(featureInfo));
            featureDiv.AppendChild(this.BuildImpendingCriteria(featureInfo));
            return featureDiv;
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
                    this.AppendChildListToNode(featuringDiv, criteriaTestMethodSpan);
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