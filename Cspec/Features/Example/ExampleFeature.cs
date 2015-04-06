namespace Cspec.Features.Example
{
    using Cspec.Framework;

    [In_order_to("see how to use the Cspec framework")]
    [As_a("developer in test")]
    [I_want("see code examples")]
    
    [Criteria(new []{ 
        "Adding 3 and 4 gives 7",
        "Second requirement",
        "Third requirement"
    })]

    public class ExampleFeature
    {
        // marker class for the feature
    }
}