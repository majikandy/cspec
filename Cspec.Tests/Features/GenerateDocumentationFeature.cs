﻿namespace Cspec.Tests.Features
{
    using Cspec.Framework;

    [In_order_to("see a nice output")]
    [As_a("dev team member")]
    [I_want("to extract features from the source code")]

    [Criteria(new[]{
        "it builds the FeaturesInfo from the attributed source code",
        "a pending scenario",
        "another pending scenario"}
    )]

    public class GenerateDocumentationFeature
    {
    }
}
