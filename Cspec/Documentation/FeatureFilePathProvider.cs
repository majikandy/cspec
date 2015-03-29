namespace Cspec.Documentation
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