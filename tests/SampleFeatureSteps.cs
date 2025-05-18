using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace SeleniumTestDemo.Tests
{
    [Binding]
    public class SampleFeatureSteps
    {
        private readonly List<int> _numbers = new();
        private int _result;

        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredNumberIntoTheCalculator(int number)
        {
            _numbers.Add(number);
        }

        [When("I press add")]
        public void WhenIPressAdd()
        {
            _result = 0;
            foreach (var n in _numbers)
                _result += n;
        }

        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(int expected)
        {
            Assert.Equal(expected, _result);
        }
    }
} 