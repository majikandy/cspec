namespace Cspec.Common
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string FormatWith(this string toFormat, params object[] args)
        {
            return string.Format(toFormat, args);
        }

        public static string TrimWhitespaceWithinHtml(this string htmlToTrim)
        {
            var regexBetweenTags = new Regex(@">(?! )\s+", RegexOptions.Compiled);
            var regexLineBreaks = new Regex(@"([\n\s])+?(?<= {2,})<", RegexOptions.Compiled);

            var html = regexBetweenTags.Replace(htmlToTrim, ">");
            html = regexLineBreaks.Replace(html, "<");
            return html.Trim();
        }
    }
}