using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Features.Example.Scenarios
{
    using NUnit.Framework;

    public class adding_0_and_0_gives_0 : ExampleFeature
    {
        [Test(Description = "adding_0_and_0_gives_0")]
        public void do_unnecessary_test()
        {
            given_an_extra_test();
            then_it_will_appear_in_superpfluous_section();
        }

        private void then_it_will_appear_in_superpfluous_section()
        {
        }

        private void given_an_extra_test()
        {
        }
    }
}
