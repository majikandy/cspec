namespace Cspec.Extractor
{
    using System;

    public class CriteriaNotSpecifiedException : Exception
    {
        public CriteriaNotSpecifiedException(string message):base(message)
        {
        }
    }
}