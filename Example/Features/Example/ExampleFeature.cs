namespace Example.Features.Example
{
    using Cspec.Framework;

    [In_order_to("see how to use the Cspec framework")]
    [As_a("developer in test")]
    [I_want("see code examples")]
    
    [Criteria(new []{ 
        "Adding 3 and 4 gives 7",
        "Adding 4 and 5 gives 9",
        "Adding 1 and 0 gives 1"
    })]

    public class ExampleFeature
    {
        // marker class for the feature
    }
}