namespace Cspec.Extractor
{
    using System.Configuration;
    using System.IO;
    using System.Reflection;

    public class FeatureFilePathProvider : IGetFeatureFilesPath
    {
        public string FeatureFilesRootPath
        {
            get
            {
                // First try to get from config
                var featureFilesRootPath = ConfigurationManager.AppSettings["featureFilesRootPath"];
                if (!string.IsNullOrEmpty(featureFilesRootPath))
                {
                    return featureFilesRootPath;
                }

                // Auto-discover Features directory by searching up from assembly location
                var assemblyLocation = Assembly.GetExecutingAssembly().Location;
                var currentDir = Path.GetDirectoryName(assemblyLocation);

                while (currentDir != null)
                {
                    var featuresPath = Path.Combine(currentDir, "Features");
                    if (Directory.Exists(featuresPath))
                    {
                        return featuresPath;
                    }

                    currentDir = Directory.GetParent(currentDir)?.FullName;
                }

                // Fallback to the old default
                return @"..\..\Features";
            }
        }
    }
}