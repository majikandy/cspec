namespace Cspec.Tests.Features.GenerateDocumentation
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Cspec.Common;
    using Cspec.Extractor;
    using Cspec.Generators;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class extract_of_info_from_itself_tests : GenerateDocumentationFeature
    {
        private Assembly assembly;
        private IEnumerable<FeatureInfo> extractedFeatures;

        [TestMethod,Description("it builds the FeaturesInfo from the attributed source code")]
        public void should_build_from_attributed_test_code ()
        {
            this.given_current_assembly();
            this.when_extracting_features();
            this.then_I_should_see_a_representation_of_this_feature();
            this.then_I_should_be_able_to_produce_this_feature_in_gherkin();
            this.then_I_should_be_able_to_produce_this_feature_in_html();
        }

        [TestMethod,Description("it builds the FeaturesInfo from the attributed source code")]
        public void should_show_you_can_have_multiple_tests_per_scenario()
        {
            this.when_without_a_given_and_a_param_of(3);
            this.then_only_exists_to_check_documentation_extraction_works();
        }

        [TestMethod, Description("something not required")]
        public void should_show_in_report_as_not_required()
        {
        }

        [Ignore, TestMethod, Description("ignored test should appear as pending")]
        public void ignore_test_appears_as_pending()
        {
        }

        private void then_only_exists_to_check_documentation_extraction_works()
        {    
        }

        private void when_without_a_given_and_a_param_of(int onlyUsedToProveTest)
        {
        }

        private void given_current_assembly()
        {
            this.assembly = this.GetType().Assembly;
        }

        private void when_extracting_features()
        {
            var featureExtractor = new FeatureExtractor(new FeatureDescriptionExtractor(), new ScenarioExtrator(new GivenWhenThensExtractor(@"..\..\Features")));
            this.extractedFeatures = featureExtractor.ExtractFeatures(this.assembly);
        }

        private void then_I_should_see_a_representation_of_this_feature()
        {
            var featureInfo = this.extractedFeatures.Single();

            Assert.AreEqual("GenerateDocumentation", featureInfo.Id);

            CheckDescription(featureInfo.AcceptanceDescription.ToList());
            CheckCriteria(featureInfo.Criteria.ToList());
            CheckPendingCriteria(featureInfo.PendingCriteria.ToList());

            Assert.AreEqual(1, featureInfo.SuperfluousCriteria.Count());
        }

        private static void CheckCriteria(List<CriteriaInfo> criteria)
        {
            Assert.AreEqual(3, criteria.Count());

            var firstCriterion = criteria.First();

            Assert.AreEqual(firstCriterion.Name,"matches tests within classes matching the criteria name without need for description attribute");
            Assert.AreEqual(firstCriterion.TestMethodName,"a_test_covering_criteria_specified_in_this_class_name");
            Assert.AreEqual(firstCriterion.GivenWhenThens.First(),"given");
            Assert.AreEqual(firstCriterion.GivenWhenThens.Second(), "when");
            Assert.AreEqual(firstCriterion.GivenWhenThens.Third(), "then");

            var secondCriterion = criteria.Second();

            Assert.AreEqual(secondCriterion.Name, "it builds the FeaturesInfo from the attributed source code");
            Assert.AreEqual(secondCriterion.TestMethodName, "should_build_from_attributed_test_code");

            Assert.AreEqual(secondCriterion.GivenWhenThens.First(), "given current assembly");
            Assert.AreEqual(secondCriterion.GivenWhenThens.Second(), "when extracting features");
            Assert.AreEqual(secondCriterion.GivenWhenThens.Third(), "then I should see a representation of this feature");
            Assert.AreEqual(secondCriterion.GivenWhenThens.Fourth(), "then I should be able to produce this feature in gherkin");
            Assert.AreEqual(secondCriterion.GivenWhenThens.Fifth(), "then I should be able to produce this feature in html");

            var thirdCriterion = criteria.Third();

            Assert.AreEqual(thirdCriterion.Name, "it builds the FeaturesInfo from the attributed source code");
            Assert.AreEqual(thirdCriterion.TestMethodName, "should_show_you_can_have_multiple_tests_per_scenario");
            Assert.AreEqual(thirdCriterion.GivenWhenThens.First(), "when without a given and a param of(3)");
            Assert.AreEqual(thirdCriterion.GivenWhenThens.Second(), "then only exists to check documentation extraction works");
        }

        private static void CheckPendingCriteria(List<string> pendingCriteria)
        {
            Assert.AreEqual(pendingCriteria.Count(), (3));
            Assert.AreEqual(pendingCriteria.First(), "a pending scenario");
            Assert.AreEqual(pendingCriteria.Second(), "another pending scenario");
            Assert.AreEqual(pendingCriteria.Third(), "ignored test should appear as pending");
        }

        private static void CheckDescription(List<string> acceptanceDescription)
        {
            Assert.AreEqual(acceptanceDescription.First(), "In order to see a nice output");
            Assert.AreEqual(acceptanceDescription.Second(), "As a dev team member");
            Assert.AreEqual(acceptanceDescription.Third(), "I want to extract features from the source code");
        }

        private void then_I_should_be_able_to_produce_this_feature_in_gherkin()
        {
            var expectedGherkin = @"Feature: Generate documentation

In order to see a nice output
As a dev team member
I want to extract features from the source code

Scenario: a test covering criteria specified in this class name
    Given
    When
    Then

Scenario: should build from attributed test code
    Given current assembly
    When extracting features
    Then I should see a representation of this feature
    Then I should be able to produce this feature in gherkin
    Then I should be able to produce this feature in html

Scenario: should show you can have multiple tests per scenario
    When without a given and a param of(3)
    Then only exists to check documentation extraction works

@ignore @pending
Scenario: a pending scenario

@ignore @pending
Scenario: another pending scenario

@ignore @pending
Scenario: ignored test should appear as pending

";
            Assert.AreEqual(
                new GherkinFeatureGenerator().Build(this.extractedFeatures), 
                expectedGherkin);
        }

        private void then_I_should_be_able_to_produce_this_feature_in_html()
        {
            var expectedHtml = @"
<div class=""container"">
    <link rel=""stylesheet"" type=""text/css"" href=""Content\bootstrap.min.css"" />
    <script src=""Scripts\jquery-1.9.1.min.js""></script>
    <script src=""Scripts\bootstrap.min.js""></script>

    <div class=""features"">
        <div class=""feature well"">
            <a class=""h2"" data-toggle=""collapse"" href=""#GenerateDocumentationCollapser"" aria-expanded=""false"" aria-controls=""GenerateDocumentationCollapser"">Generate documentation</a>
            <div id=""GenerateDocumentationCollapser"" class=""collapse"">
                <br />
                <div class=""description well"">
                    <div>In order to see a nice output</div>
                    <div>As a dev team member</div>
                    <div>I want to extract features from the source code</div>
                </div>
                <div class=""criteria text-success"">
                    <h4>Current</h4>
                    <strong>
                        <div class=""criterion-description"">
                            <a href=""#criterion-collapser-1"" aria-controls=""criterion-collapser-1"" data-toggle=""collapse"" aria-expanded=""false"">- matches tests within classes matching the criteria name without need for description attribute</a> 
                        </div>
                    </strong>
                    <div id=""criterion-collapser-1"" class=""collapse criterion"">
                        <div class=""scenario"">
                            <div class=""test-method-name"">--> (method) a test covering criteria specified in this class name</div>
                            <div class=""givenWhenThens"">
                                <div class=""step"">----> Given</div>
                                <div class=""step"">----> When</div>
                                <div class=""step"">----> Then</div>
                            </div>
                        </div>
                    </div>
                    <strong>
                        <div class=""criterion-description"">
                            <a href=""#criterion-collapser-2""  aria-controls=""criterion-collapser-2"" data-toggle=""collapse"" aria-expanded=""false"">- it builds the FeaturesInfo from the attributed source code</a>
                        </div>
                    </strong>
                    <div id=""criterion-collapser-2"" class=""collapse criterion"">
                        <div class=""scenario"">
                            <div class=""test-method-name"">--> (method) should build from attributed test code</div>
                            <div class=""givenWhenThens"">
                                <div class=""step"">----> Given current assembly</div>
                                <div class=""step"">----> When extracting features</div>
                                <div class=""step"">----> Then I should see a representation of this feature</div>
                                <div class=""step"">----> Then I should be able to produce this feature in gherkin</div>
                                <div class=""step"">----> Then I should be able to produce this feature in html</div>
                            </div>
                        </div>
                        <div class=""scenario"">
                            <div class=""test-method-name"">--> (method) should show you can have multiple tests per scenario</div>
                            <div class=""givenWhenThens"">
                                <div class=""step"">----> When without a given and a param of(3)</div>
                                <div class=""step"">----> Then only exists to check documentation extraction works</div>
                            </div>
                        </div>
                    </div>
                </div>
            
                <div class=""pending-criteria text-warning bg-warning"">
                    <h4>Pending:</h4>
                    <div class=""criteria"">a pending scenario</div>
                    <div class=""criteria"">another pending scenario</div>
                    <div class=""criteria"">ignored test should appear as pending</div>
                </div>
                <div class=""superfluous-criteria text-warning bg-error"">
                    <h4>Superfluous:</h4>
                    <div class=""criteria"">something not required</div>
                </div>
            </div>
        </div>
    </div>
</div>
";

            File.WriteAllText(@"c:\temp\cspec.features.html", expectedHtml);

            Assert.AreEqual(
                new HtmlFeatureGenerator().Build(this.extractedFeatures), 
                expectedHtml.TrimWhitespaceWithinHtml());
        }
    }
}