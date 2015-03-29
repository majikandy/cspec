namespace Cspec.Documentation
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Cspec.Common;

    public class GivenWhenThensExtractor : IExtractGivenWhenThens
    {
        private readonly IGetFeatureFilesPath featuresConfig;

        public GivenWhenThensExtractor(IGetFeatureFilesPath featuresConfig)
        {
            this.featuresConfig = featuresConfig;
        }

        public IEnumerable<string> GetGivenWhenThens(string methodNameContainingGivenWhenThens, string featureClassName)
        {
            var featureCs = this.GetFeatureCsFilePath(featureClassName);

            var featureFileSourcecodeLines = File.ReadAllLines(featureCs);
            var givenWhenThens = new List<string>();
            var methodFound = false;
            foreach (var line in featureFileSourcecodeLines)
            {
                if (!methodFound && !line.Contains(methodNameContainingGivenWhenThens))
                {
                    continue;
                }
                methodFound = true;
                if (line.Trim() == "}")
                {
                    break;
                }
                if (line.Contains(methodNameContainingGivenWhenThens) || line.Trim() == "{")
                {
                    continue;
                }
                var stepName = line.Trim().Replace("this.", "").Replace("_", " ").Replace("();", string.Empty);
                if (stepName.EndsWith(";"))
                {
                    stepName = stepName.Substring(0, stepName.Length - 1);
                }
                givenWhenThens.Add(stepName);
            }
            return givenWhenThens;
        }

        private string GetFeatureCsFilePath(string featureClassName)
        {
            var featureFilesRootPath = this.featuresConfig.FeatureFilesRootPath ?? @"..\..\Features";

            var featureCs =
                Directory.GetFiles(featureFilesRootPath, featureClassName + ".cs", SearchOption.AllDirectories)
                    .SingleOrDefault();

            if (featureCs == null)
            {
                throw new SpecException(@"Features not found at '{0}'. Ensure all features are located within a \Features\ folder at the root of the test project, or override this setting by supplying the following app setting for the project <add key='featureFilesRootPath' value='path_here' />"
                    .FormatWith(featureFilesRootPath));
            }
            return featureCs;
        }
    }
}