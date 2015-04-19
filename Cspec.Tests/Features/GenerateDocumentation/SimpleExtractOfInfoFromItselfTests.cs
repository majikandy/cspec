namespace Cspec.Tests.Features.GenerateDocumentation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Cspec.Common;
    using Cspec.Extractor;
    using Cspec.Generators;

    using NUnit.Framework;

    /// <summary>
    /// The simple extract of info from itself tests.
    /// </summary>
    public class SimpleExtractOfInfoFromItselfTests : GenerateDocumentationFeature
    {
        /// <summary>
        /// The assembly.
        /// </summary>
        private Assembly assembly;

        /// <summary>
        /// The extracted features.
        /// </summary>
        private IEnumerable<FeatureInfo> extractedFeatures;

        /// <summary>
        /// The should_build_from_attributed_test_code.
        /// </summary>
        [Test(Description = "it builds the FeaturesInfo from the attributed source code")]
        public void should_build_from_attributed_test_code ()
        {
            this.given_current_assembly();
            this.when_extracting_features();
            this.then_I_should_see_a_representation_of_this_feature();
            this.then_I_should_be_able_to_produce_this_feature_in_gherkin();
            this.then_I_should_be_able_to_produce_this_feature_in_html();
        }

        /// <summary>
        /// The should_show_you_can_have_multiple_tests_per_scenario.
        /// </summary>
        [Test(Description = "it builds the FeaturesInfo from the attributed source code")]
        public void should_show_you_can_have_multiple_tests_per_scenario()
        {
            this.when_without_a_given_and_a_param_of(3);
            this.then_only_exists_to_check_docuementation_extraction_works();
        }

        /// <summary>
        /// The should_show_in_report_as_not_required.
        /// </summary>
        [Test(Description = "something not required")]
        public void should_show_in_report_as_not_required()
        {
        }

        /// <summary>
        /// The then_only_exists_to_check_docuementation_extraction_works.
        /// </summary>
        private void then_only_exists_to_check_docuementation_extraction_works()
        {    
        }

        /// <summary>
        /// The when_without_a_given_and_a_param_of.
        /// </summary>
        /// <param name="onlyUsedToProveTest">
        /// The only used to prove test.
        /// </param>
        private void when_without_a_given_and_a_param_of(int onlyUsedToProveTest)
        {
        }

        /// <summary>
        /// The given_current_assembly.
        /// </summary>
        private void given_current_assembly()
        {
            this.assembly = this.GetType().Assembly;
        }

        /// <summary>
        /// The when_extracting_features.
        /// </summary>
        private void when_extracting_features()
        {
            var featureExtractor = new FeatureExtractor(new FeatureDescriptionExtractor(), new ScenarioExtrator(new GivenWhenThensExtractor(new FeatureFilePathProvider())));
            this.extractedFeatures = featureExtractor.ExtractFeatures(this.assembly);
        }

        /// <summary>
        /// The then_ i_should_see_a_representation_of_this_feature.
        /// </summary>
        private void then_I_should_see_a_representation_of_this_feature()
        {
            var featureInfo = this.extractedFeatures.Single();
            
            Assert.That(featureInfo.AcceptanceDescription.First(), Is.EqualTo("In order to see a nice output"));
            Assert.That(featureInfo.AcceptanceDescription.Skip(1).First(), Is.EqualTo("As a dev team member"));
            Assert.That(featureInfo.AcceptanceDescription.Skip(2).First(), Is.EqualTo("I want to extract features from the source code"));
            
            Assert.That(featureInfo.Name, Is.EqualTo("GenerateDocumentation"));

            Assert.That(featureInfo.PendingCriteria.Count(), Is.EqualTo(2));
            Assert.That(featureInfo.PendingCriteria.First(), Is.EqualTo("a pending scenario"));
            Assert.That(featureInfo.PendingCriteria.Skip(1).First(), Is.EqualTo("another pending scenario"));

            Assert.That(featureInfo.Criteria.Count(), Is.EqualTo(2));
            Assert.That(featureInfo.Criteria.First().Name, Is.EqualTo("it builds the FeaturesInfo from the attributed source code"));
            Assert.That(featureInfo.Criteria.First().TestMethodName, Is.EqualTo("should_build_from_attributed_test_code"));
            
            Assert.That(featureInfo.Criteria.First().GivenWhenThens.First(), Is.EqualTo("given current assembly"));
            Assert.That(featureInfo.Criteria.First().GivenWhenThens.Skip(1).First(), Is.EqualTo("when extracting features"));
            Assert.That(featureInfo.Criteria.First().GivenWhenThens.Skip(2).First(), Is.EqualTo("then I should see a representation of this feature"));
            Assert.That(featureInfo.Criteria.First().GivenWhenThens.Skip(3).First(), Is.EqualTo("then I should be able to produce this feature in gherkin"));
            Assert.That(featureInfo.Criteria.First().GivenWhenThens.Skip(4).First(), Is.EqualTo("then I should be able to produce this feature in html"));

            Assert.That(featureInfo.Criteria.Skip(1).First().Name, Is.EqualTo("it builds the FeaturesInfo from the attributed source code"));
            Assert.That(featureInfo.Criteria.Skip(1).First().TestMethodName, Is.EqualTo("should_show_you_can_have_multiple_tests_per_scenario"));
            Assert.That(featureInfo.Criteria.Skip(1).First().GivenWhenThens.First(), Is.EqualTo("when without a given and a param of(3)"));
            Assert.That(featureInfo.Criteria.Skip(1).First().GivenWhenThens.Skip(1).First(), Is.EqualTo("then only exists to check docuementation extraction works"));

            Assert.That(featureInfo.SuperfluousCriteria.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// The then_ i_should_be_able_to_produce_this_feature_in_gherkin.
        /// </summary>
        private void then_I_should_be_able_to_produce_this_feature_in_gherkin()
        {
            var expectedGherkin = @"Feature: GenerateDocumentation

In order to see a nice output
As a dev team member
I want to extract features from the source code

Scenario: should build from attributed test code
    given current assembly
    when extracting features
    then I should see a representation of this feature
    then I should be able to produce this feature in gherkin
    then I should be able to produce this feature in html

Scenario: should show you can have multiple tests per scenario
    when without a given and a param of(3)
    then only exists to check docuementation extraction works

@ignore @pending
Scenario: a pending scenario

@ignore @pending
Scenario: another pending scenario

";
            Assert.That(
                new GherkinFeatureGenerator().Build(this.extractedFeatures), 
                Is.EqualTo(expectedGherkin));
        }

        /// <summary>
        /// The then_ i_should_be_able_to_produce_this_feature_in_html.
        /// </summary>
        private void then_I_should_be_able_to_produce_this_feature_in_html()
        {
            var expectedHtml = @"
<div class=""container"">
    <link rel=""stylesheet"" type=""text/css"" href=""bootstrap.min.css"" />
    <div class=""features"">
        <div class=""feature well"">
            <h2>GenerateDocumentation</h2>
            <div class=""description well"">
                <div>In order to see a nice output</div>
                <div>As a dev team member</div>
                <div>I want to extract features from the source code</div>
            </div>
            <div class=""criteria text-success"">
                <h4>Current</h4>
                <div class=""criterion"">
                    <strong><div class=""criterion-description"">-it builds the FeaturesInfo from the attributed source code</div></strong>
                    <div class=""scenario"">
                        <div class=""test-method-name"">--> should build from attributed test code</div>
                        <div class=""givenWhenThens"">
                            <div class=""step"">----> given current assembly</div>
                            <div class=""step"">----> when extracting features</div>
                            <div class=""step"">----> then I should see a representation of this feature</div>
                            <div class=""step"">----> then I should be able to produce this feature in gherkin</div>
                            <div class=""step"">----> then I should be able to produce this feature in html</div>
                        </div>
                    </div>
                    <div class=""scenario"">
                        <div class=""test-method-name"">--> should show you can have multiple tests per scenario</div>
                        <div class=""givenWhenThens"">
                            <div class=""step"">----> when without a given and a param of(3)</div>
                            <div class=""step"">----> then only exists to check docuementation extraction works</div>
                        </div>
                    </div>
                </div>
            </div>
            <div class=""pending-criteria text-warning bg-warning"">
                <h4>Pending:</h4>
                <div class=""criteria"">a pending scenario</div>
                <div class=""criteria"">another pending scenario</div>
            </div>
            <div class=""superfluous-criteria text-warning bg-error"">
                <h4>Superfluous:</h4>
                <div class=""criteria"">something not required</div>
            </div>
        </div>
    </div>
</div>
";

            Assert.That(
                new HtmlFeatureGenerator().Build(this.extractedFeatures), 
                Is.EqualTo(expectedHtml.TrimWhitespaceWithinHtml()));
        }
    }
}