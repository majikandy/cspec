namespace Cspec.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using CSpec.DocGen;
    using CSpec.Framework;

    using NUnit.Framework;

    [In_order_to("see a nice output")]
    [As_a("dev team member")]
    [I_want("to extract features from the source code")]

    [PendingScenarios(@"a pending scenario
                        another pending scenario")]

    public class GenerateDocumentationFeature
    {
    }

    public class SimpleExtractOfInfoFromThisTestItself : GenerateDocumentationFeature
    {
        private Assembly assembly;
        private IEnumerable<FeatureInfo> extractedFeatures;

        [TestShowing("it builds the FeaturesInfo from the attributed source code")]
        public void should_build_from_attributed_test_code()
        {
            given_current_assembly();
            when_extracting_features();
            then_I_should_see_a_representation_of_this_feature();
        }

        [TestShowing("it builds the FeaturesInfo from the attributed source code")]
        public void should_show_you_can_have_multiple_tests_per_scenario()
        {
            when_without_a_given();
            then_only_exists_to_check_docuementation_extraction_works();
        }

        private void then_only_exists_to_check_docuementation_extraction_works()
        {    
        }

        private void when_without_a_given()
        {
        }

        private void given_current_assembly()
        {
            this.assembly = this.GetType().Assembly;
        }

        private void when_extracting_features()
        {
            var featureExtractor = new FeatureExtractor();
            this.extractedFeatures = featureExtractor.ExtractFeatures(this.assembly);
        }

        private void then_I_should_see_a_representation_of_this_feature()
        {
            var featureInfo = this.extractedFeatures.Single();
            
            Assert.That(featureInfo.AcceptanceDescription.First(), Is.EqualTo("In order to see a nice output"));
            Assert.That(featureInfo.AcceptanceDescription.Skip(1).First(), Is.EqualTo("As a dev team member"));
            Assert.That(featureInfo.AcceptanceDescription.Skip(2).First(), Is.EqualTo("I want to extract features from the source code"));
            
            Assert.That(featureInfo.Name, Is.EqualTo("GenerateDocumentation"));

            Assert.That(featureInfo.PendingScenarios.Count(), Is.EqualTo(2));
            Assert.That(featureInfo.PendingScenarios.First(), Is.EqualTo("a pending scenario"));
            Assert.That(featureInfo.PendingScenarios.Skip(1).First(), Is.EqualTo("another pending scenario"));

            Assert.That(featureInfo.Scenarios.Count(), Is.EqualTo(2));
            Assert.That(featureInfo.Scenarios.First().Name, Is.EqualTo("it builds the FeaturesInfo from the attributed source code"));
            Assert.That(featureInfo.Scenarios.First().TestMethodName, Is.EqualTo("should_build_from_attributed_test_code"));
            
            Assert.That(featureInfo.Scenarios.First().GivenWhenThens.First(), Is.EqualTo("given current assembly"));
            Assert.That(featureInfo.Scenarios.First().GivenWhenThens.Skip(1).First(), Is.EqualTo("when extracting features"));
            Assert.That(featureInfo.Scenarios.First().GivenWhenThens.Skip(2).First(), Is.EqualTo("then I should see a representation of this feature"));

            Assert.That(featureInfo.Scenarios.Skip(1).First().Name, Is.EqualTo("it builds the FeaturesInfo from the attributed source code"));
            Assert.That(featureInfo.Scenarios.Skip(1).First().TestMethodName, Is.EqualTo("should_show_you_can_have_multiple_tests_per_scenario"));
        }
    }
}
