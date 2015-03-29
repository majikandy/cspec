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
                stringBuilder.AppendLine("Feature: {0}".FormatWith(featureInfo.Name));
                stringBuilder.AppendLine();
                foreach (var line in featureInfo.AcceptanceDescription)
                {
                    stringBuilder.AppendLine(line);
                }
                stringBuilder.AppendLine();

                foreach (var scenario in featureInfo.Scenarios)
                {
                    stringBuilder.AppendLine("Scenario: {0}".FormatWith(scenario.TestMethodName.Replace("_", " ")));
                    foreach (var step in scenario.GivenWhenThens)
                    {
                        stringBuilder.AppendLine("    {0}".FormatWith(step));
                    }
                    stringBuilder.AppendLine();
                }

                foreach (var pendingScenario in featureInfo.PendingScenarios)
                {
                    stringBuilder.AppendLine("@ignore @pending");
                    stringBuilder.AppendLine("Scenario: {0}".FormatWith(pendingScenario));
                    stringBuilder.AppendLine();
                }
            }

            return stringBuilder.ToString();
        }
    }
}