namespace Cspec.Common
{
    using System.Web.UI;

    public static class HtmlTextWriterExtensions
    {
        public static void WriteTag(this HtmlTextWriter writer, HtmlTextWriterTag tag, string tagText, string classAttributeText)
        {
            writer.OpenTag(tag, tagText, classAttributeText);
            writer.RenderEndTag();
        }

        public static void WriteTag(this HtmlTextWriter writer, HtmlTextWriterTag tag, string tagText)
        {
            writer.OpenTag(tag, tagText);
            writer.RenderEndTag();
        }

        public static void OpenTag(this HtmlTextWriter writer, HtmlTextWriterTag tag, string tagText, string classAttributeText)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, classAttributeText);
            writer.OpenTag(tag, tagText);
        }

        public static void OpenTag(this HtmlTextWriter writer, HtmlTextWriterTag tag, string tagText)
        {
            writer.RenderBeginTag(tag);
            writer.Write(tagText);
        }

        public static void OpenDiv(this HtmlTextWriter writer, string classAttributeText)
        {
            writer.OpenTag(HtmlTextWriterTag.Div, string.Empty, classAttributeText);
        }

        public static void CloseTag(this HtmlTextWriter writer)
        {
            writer.RenderEndTag();
        }

        public static void WriteStylesheet(this HtmlTextWriter writer, string cssPath)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, cssPath);
            writer.RenderBeginTag(HtmlTextWriterTag.Link);
            writer.RenderEndTag();
        }

        public static void WriteScript(this HtmlTextWriter writer, string jsPath)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Src, jsPath);
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.RenderEndTag();
        }
    }
}