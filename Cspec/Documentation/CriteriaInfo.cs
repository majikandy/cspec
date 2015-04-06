namespace Cspec.Documentation
{
    using System.Collections.Generic;

    public class CriteriaInfo
    {
        public CriteriaInfo(string name, string testMethodName, IEnumerable<string> givenWhenThens)
        {
            this.Name = name;
            this.TestMethodName = testMethodName;
            this.GivenWhenThens = givenWhenThens;
        }

        public string Name { get; private set; }
        public string TestMethodName { get; private set; }
        public IEnumerable<string> GivenWhenThens { get; private set; }
    }
}