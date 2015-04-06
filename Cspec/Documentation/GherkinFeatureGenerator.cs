namespace Cspec.Documentation
{
    using System.Collections.Generic;
    using System.Text;

    using Cspec.Common;

    public class GherkinFeatureGenerator : IGenerateFeatures
    {
        public string BuildFeatureDocumentation(IEnumerable<FeatureInfo> features)
        {
            var stringBuilder = new StringBuilder();

            foreach (var featureInfo in features)
            {
                stringBuilder.AppendLine("Feature: {0}".With(featureInfo.Name));
                stringBuilder.AppendLine();
                foreach (var line in featureInfo.AcceptanceDescription)
                {
                    stringBuilder.AppendLine(line);
                }
                stringBuilder.AppendLine();

                foreach (var scenario in featureInfo.Criteria)
                {
                    stringBuilder.AppendLine("Scenario: {0}".With(scenario.TestMethodName.Replace("_", " ")));
                    foreach (var step in scenario.GivenWhenThens)
                    {
                        stringBuilder.AppendLine("    {0}".With(step));
                    }
                    stringBuilder.AppendLine();
                }

                foreach (var pendingScenario in featureInfo.PendingCriteria)
                {
                    stringBuilder.AppendLine("@ignore @pending");
                    stringBuilder.AppendLine("Scenario: {0}".With(pendingScenario));
                    stringBuilder.AppendLine();
                }
            }

            return stringBuilder.ToString();
        }
    }
}