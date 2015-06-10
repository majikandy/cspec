namespace Cspec.Tests.Features
{
    using Cspec.Framework;

    [In_order_to("see a nice output")]
    [As_a("dev team member")]
    [I_want("to extract features from the source code")]

    [Criteria(new[]{
        "it builds the FeaturesInfo from the attributed source code",
        "a pending scenario",
        "another pending scenario",
        "ignored test should appear as pending",
        "matches tests, within classes matching the criteria name without need for description attribute"
    }
    )]

    public class GenerateDocumentationFeature
    {
    }
}
