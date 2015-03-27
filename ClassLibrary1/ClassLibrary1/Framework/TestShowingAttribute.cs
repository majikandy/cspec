namespace CSpec.Framework
{
    using NUnit.Framework;

    public class TestShowingAttribute : TestAttribute
    {
        public TestShowingAttribute(string functionaility)
        {
            this.Functionality = functionaility;
        }

        public string Functionality { get; set; }
    }
}