namespace Cspec.Common
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string With(this string toFormat, params object[] args)
        {
            return string.Format(toFormat, args);
        }

        public static string TrimWhitespaceWithinHtml(this string htmlToTrim)
        {
            var html = Regex.Replace(htmlToTrim, @"\n|\t|\r", string.Empty);
            html = Regex.Replace(html, @">\s+<", "><").Trim();
            html = Regex.Replace(html, @"\s{2,}", " ");
            return html.Trim();
        }

        public static string WithSpacesInsteadOfUnderscores(this string theString)
        {
            return theString.Replace("_", " ");
        }

        public static string RemoveWhitespace(this string theString)
        {
            return theString.Replace(" ", string.Empty);
        }
    }
}