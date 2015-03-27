namespace CSpec
{
    public static class StringExtensions
    {
        public static string FormatWith(this string toFormat, params object[] args)
        {
            return string.Format(toFormat, args);
        }
    }
}