namespace Cspec.Extractor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Cspec.Common;

    public class GivenWhenThensExtractor : IExtractGivenWhenThens
    {
        private readonly string featuresFolderPath;

        public GivenWhenThensExtractor(string featuresFolderPath)
        {
            this.featuresFolderPath = featuresFolderPath;
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
                    if (!methodFound && !line.RemoveWhitespace().Contains(methodNameContainingGivenWhenThens + "("))
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

                    var stepName = line.Trim()
                        .Replace("this.", string.Empty)
                        .Replace("_", " ")
                        .Replace("();", string.Empty);

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

            throw new CspecException(
                "{0} wasn't found in any of the features source files, check that the files inherit from the <FeatureName>Feature class and are in files by the same name as their class names");
        }

        private IEnumerable<string> GetCsFilePaths(IEnumerable<Type> derivationsClassNames)
        {
            var featureFilesRootPath = this.featuresFolderPath ?? @"..\..\Features";
            Console.WriteLine("Using path for features: " + featureFilesRootPath);

            var sourceFiles = derivationsClassNames
                .Select(x => Directory.GetFiles(featureFilesRootPath, x.Name + ".cs", SearchOption.AllDirectories)
                .SingleOrDefault()).ToList();
            
            if (sourceFiles.Any(x => x == null))
            {
                var affectedFiles = derivationsClassNames.Select(x => x.Name).Except(sourceFiles.Select(x => Path.GetFileNameWithoutExtension(x)));
                throw new CspecException("Test classes that derive from <*.>Feature.cs classes must be in their own files and have a matching file name (Cspec requires this):\n {0}".With(string.Join("\n ", affectedFiles)));
            }

            var missingClassFiles = derivationsClassNames.Select(d => d.Name.ToLower())
                .Except(sourceFiles.Select(s => Path.GetFileNameWithoutExtension(s).ToLower()));
            
            if (missingClassFiles.Any())
            {
                var allClasses = string.Join("\n\n", missingClassFiles.Select(x => x));
                throw new CspecException("The following class names did not have file names that match the class name (Cspec requires this):\n {0}".With(allClasses) );
            }

            return sourceFiles;
        }
    }
}