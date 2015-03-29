namespace Cspec.Documentation
{
    using System.Collections.Generic;

    public interface IExtractGivenWhenThens
    {
        IEnumerable<string> GetGivenWhenThens(
            string methodNameContainingGivenWhenThens,
            string featureClassName);
    }
}