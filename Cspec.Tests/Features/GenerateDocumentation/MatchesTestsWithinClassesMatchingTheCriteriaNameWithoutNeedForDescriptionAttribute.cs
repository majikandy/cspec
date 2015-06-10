namespace Cspec.Tests.Features.GenerateDocumentation
{
    using NUnit.Framework;

    public class MatchesTestsWithinClassesMatchingTheCriteriaNameWithoutNeedForDescriptionAttribute : GenerateDocumentationFeature
    {
        [Test]
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
