namespace CSpec.Features.Example
{
    using CSpec.Framework;

    [In_order_to("see how to use the Cspec framework")]
    [As_a("developer in test")]
    [I_want("see code examples")]
    
    [PendingScenarios(@" 
        Second requirement
        Third requirement
    ")]

    public class ExampleFeature
    {
        // marker class for the feature
    }
}