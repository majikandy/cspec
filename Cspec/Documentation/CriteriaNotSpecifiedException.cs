namespace Cspec.Documentation
{
    using System;

    public class CriteriaNotSpecifiedException : Exception
    {
        public CriteriaNotSpecifiedException(string message):base(message)
        {
        }
    }
}