namespace CSpec.DocGen
{
    using HtmlAgilityPack;

    public static class HtmlDocumentExtensions
    {
        public static HtmlNode CreateElementWithText(this HtmlDocument htmlDoc, string name, string text)
        {
            var featureName = htmlDoc.CreateElement(name);
            featureName.InnerHtml = text;
            return featureName;
        }

        public static HtmlNode CreateElementWithAttribute(this HtmlDocument htmlDoc, string name, string attributeName, string attributeValue)
        {
            var featureDiv = htmlDoc.CreateElement(name);
            featureDiv.SetAttributeValue(attributeName, attributeValue);
            return featureDiv;
        }

        public static HtmlNode CreateElementWithAttributeAndText(this HtmlDocument htmlDoc, string name, string attributeName, string attributeValue, string text)
        {
            var element = htmlDoc.CreateElementWithAttribute(name, attributeName, attributeValue);
            element.InnerHtml = text;
            return element;
        }
    }
}