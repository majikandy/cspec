namespace Cspec.Tests.Features.GenerateDocumentation
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class matches_tests_within_classes_matching_the_criteria_name_without_need_for_description_attribute : GenerateDocumentationFeature
    {
        [TestMethod]
        public void a_test_covering_criteria_specified_in_this_class_name()
        {
            given();
            when();
            then();
        }

        private void given()
        {
        }

        private void when()
        {
        }

        private void then()
        {
        }
    }
}
