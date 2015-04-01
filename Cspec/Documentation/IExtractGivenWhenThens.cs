namespace Cspec.Documentation
{
    using System;
    using System.Collections.Generic;

    public interface IExtractGivenWhenThens
    {
        IEnumerable<string> GetGivenWhenThens(string methodNameContainingGivenWhenThens, IEnumerable<Type> featureClassName);
    }
}