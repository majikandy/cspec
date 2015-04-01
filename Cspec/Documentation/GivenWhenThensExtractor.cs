namespace Cspec.Documentation
{
    using System;
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

        public IEnumerable<string> GetGivenWhenThens(string methodNameContainingGivenWhenThens, IEnumerable<Type> allDerivationsOfFeature)
        {
            var featureCsFiles = this.GetCsFilePaths(allDerivationsOfFeature);

            foreach (var featureCs in featureCsFiles)
            {
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
                if (methodFound)
                {
                    return givenWhenThens;
                }
            }
            throw new SpecException(
                "{0} wasn't found in any of the features source files, check that the files inherit from the <FeatureName>Feature class and are in files by the same name as their class names");
        }

        private IEnumerable<string> GetCsFilePaths(IEnumerable<Type> derivationsClassNames)
        {
            var featureFilesRootPath = this.featuresConfig.FeatureFilesRootPath ?? @"..\..\Features";

            var sourceFiles = derivationsClassNames.Select(x => Directory.GetFiles(featureFilesRootPath, x.Name + ".cs", SearchOption.AllDirectories)
                    .SingleOrDefault());

            if (sourceFiles.Any(x => x == null))
            {
                throw new SpecException(@"Features not found at '{0}'. Ensure all features are located within a \Features\ folder at the root of the test project, or override this setting by supplying the following app setting for the project <add key='featureFilesRootPath' value='path_here' />. Also check that the scenario classes inherit from the <FeatureName>Feature.cs marker class and they themselves are in file names by the same name as the class name.. eg LoginTests inside LoginTests.cs inheriting from LoginFeature (where LoginFeature.cs has the In_order_to, As_a, I_want in it)"
                    .FormatWith(featureFilesRootPath));
            }
            return sourceFiles;
        }
    }
}