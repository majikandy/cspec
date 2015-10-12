using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Features
{
    using System.IO;
    using System.Reflection;

    using Cspec.Extractor;
    using Cspec.Generators;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildDocs
    {
        [TestMethod]
        public void generate_features()
        {
            var features = new FeatureExtractor(@"..\..\Features").ExtractFeatures(Assembly.GetExecutingAssembly());
            File.WriteAllText(@"..\..\Features.html", new HtmlFeatureGenerator().Build(features));
            File.WriteAllText(@"..\..\Features.txt", new GherkinFeatureGenerator().Build(features));
        }
    }
}
