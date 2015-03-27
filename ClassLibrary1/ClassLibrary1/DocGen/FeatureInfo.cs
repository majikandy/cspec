namespace CSpec.DocGen
{
    using System.Collections.Generic;

    public class FeatureInfo
    {
        public string Name { get; set; }
        public IEnumerable<string> AcceptanceDescription { get; set; }
        public IEnumerable<Scenario> Scenarios { get; set; }
        public IEnumerable<string> PendingScenarios { get; set; }
    }

    public class Scenario
    {
        public Scenario(string name, string testMethodName, List<string> givenWhenThens)
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