namespace Cspec.Generator
{
    using System;
    using System.IO;
    using System.Reflection;

    using Cspec.Generators;

    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:\n cspec-generator.exe <report type> <assembly file path> <Features source folder path> <output file path> \n\n eg. cspec-generator.exe HTML bin\\release\\AcceptanceTests.dll Features\\ Documentation\\Features.html");
                return;
            }
            var generatorType = args[0];
            var assemblyFilePath = args[1];
            var featuresFolderPath = args[2];
            var outputHtmlFilePath = args[3];

            if (generatorType.ToLower() == "html")
            {
                File.WriteAllText(outputHtmlFilePath, new HtmlFeatureGenerator().Build(Assembly.LoadFrom(assemblyFilePath), featuresFolderPath));
            }

            if (generatorType.ToLower() == "gherkin")
            {
                File.WriteAllText(outputHtmlFilePath, new GherkinFeatureGenerator().Build(Assembly.LoadFrom(assemblyFilePath), featuresFolderPath));
            }
        }
    }
}
