namespace Cspec.Extractor
{
    using System.Configuration;

    public class FeatureFilePathProvider : IGetFeatureFilesPath
    {
        public string FeatureFilesRootPath
        {
            get
            {
                var featureFilesRootPath = ConfigurationManager.AppSettings["featureFilesRootPath"];
                return featureFilesRootPath;
            }
        }
    }
}