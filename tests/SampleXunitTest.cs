using Xunit;

namespace SeleniumTestDemo.Tests
{
    public class SampleXunitTest
    {
        [Fact]
        public void Addition_Works()
        {
            int a = 2;
            int b = 3;
            Assert.Equal(5, a + b);
        }
    }
} 