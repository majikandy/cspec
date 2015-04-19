namespace Example.Features.Example.Scenarios
{
    using NUnit.Framework;

    public class adding_3_and_4_gives_7 : ExampleFeature
    {
        private int firstNumber;
        private int secondNumber;
        private int result;

        [Test(Description = "Adding 3 and 4 gives 7")]
        public void adding_3_to_4_should_give_me_7()
        {
            this.given_first_numer_of(4);
            this.given_second_number_of(3);
            this.when_I_add_them();
            this.then_result_should_be(7);
        }

        [Test(Description = "Adding 3 and 4 gives 7")]
        public void adding_4_to_3_should_give_me_7()
        {
            this.given_first_numer_of(3);
            this.given_second_number_of(4);
            this.when_I_add_them();
            this.then_result_should_be(7);
        }

        private void given_first_numer_of(int number)
        {
            this.firstNumber = number;
        }

        private void given_second_number_of(int number)
        {
            this.secondNumber = number;
        }

        private void when_I_add_them()
        {
            this.result = this.firstNumber + this.secondNumber;
        }

        private void then_result_should_be(int expectedResult)
        {
            Assert.That(this.result, Is.EqualTo(expectedResult));
        }
    }
}
