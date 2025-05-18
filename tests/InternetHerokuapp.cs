using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTestDemo.Tests
{
    public class InternetHerokuappTests
    {
        private readonly ITestOutputHelper _output;

        public InternetHerokuappTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task InternetHerokuappNavLinks_Work()
        {
            string baseUrl = "https://the-internet.herokuapp.com";

            _output.WriteLine($"📘 Nav Test Log - {DateTime.Now}\n");

            IWebDriver driver = new ChromeDriver();

            try
            {
                driver.Navigate().GoToUrl(baseUrl);
                await Task.Delay(2000);

                // Find all <li> elements inside the <ul>
                var listItems = driver.FindElements(By.CssSelector("#content li"));

                // Log the count of <li> elements found
                _output.WriteLine($"Found {listItems.Count} <li> elements in the <ul>.");

                // Optionally, iterate through the list items and log their text
                foreach (var item in listItems)
                {
                    _output.WriteLine($" - {item.Text}");
                }
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}
