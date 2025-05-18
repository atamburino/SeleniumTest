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

        [Fact]
        public async Task AddRemoveElements_Work()
        {
            string baseUrl = "https://the-internet.herokuapp.com/add_remove_elements/";
            _output.WriteLine($"📘 Add/Remove Elements Test Log - {DateTime.Now}\n");

            IWebDriver driver = new ChromeDriver();

            try
            {
                // Navigate to the Add/Remove Elements page
                driver.Navigate().GoToUrl(baseUrl);
                await Task.Delay(2000);

                // Find and click the Add Element button
                var addButton = driver.FindElement(By.CssSelector("button[onclick='addElement()']"));
                _output.WriteLine("Found Add Element button");
                addButton.Click();
                _output.WriteLine("Clicked Add Element button");
                await Task.Delay(1000);

                // Verify the Delete button was added
                var deleteButton = driver.FindElement(By.CssSelector("button.added-manually"));
                _output.WriteLine("Found Delete button");
                Assert.True(deleteButton.Displayed, "Delete button should be visible");

                // Click the Delete button
                deleteButton.Click();
                _output.WriteLine("Clicked Delete button");
                await Task.Delay(1000);

                // Verify the Delete button was removed
                var deleteButtons = driver.FindElements(By.CssSelector("button.added-manually"));
                Assert.Empty(deleteButtons);
                _output.WriteLine("Verified Delete button was removed");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error: {ex.Message}");
                throw;
            }
            finally
            {
                driver.Quit();
            }
        }

        [Fact]
        public async Task AddRemoveMultipleElements_Work()
        {
            string baseUrl = "https://the-internet.herokuapp.com/add_remove_elements/";
            _output.WriteLine($"📘 Add/Remove Multiple Elements Test Log - {DateTime.Now}\n");

            IWebDriver driver = new ChromeDriver();

            try
            {
                // Navigate to the Add/Remove Elements page
                driver.Navigate().GoToUrl(baseUrl);
                await Task.Delay(2000);

                // Add 5 elements
                var addButton = driver.FindElement(By.CssSelector("button[onclick='addElement()']"));
                _output.WriteLine("Found Add Element button");

                for (int i = 1; i <= 5; i++)
                {
                    addButton.Click();
                    _output.WriteLine($"Added element {i}");
                    await Task.Delay(500);
                }

                // Verify 5 delete buttons were added
                var deleteButtons = driver.FindElements(By.CssSelector("button.added-manually"));
                Assert.Equal(5, deleteButtons.Count);
                _output.WriteLine($"Verified {deleteButtons.Count} Delete buttons are present");

                // Delete all buttons one by one
                for (int i = deleteButtons.Count; i > 0; i--)
                {
                    var currentDeleteButtons = driver.FindElements(By.CssSelector("button.added-manually"));
                    currentDeleteButtons[0].Click();
                    _output.WriteLine($"Deleted element {i}");
                    await Task.Delay(500);
                }

                // Verify all delete buttons were removed
                deleteButtons = driver.FindElements(By.CssSelector("button.added-manually"));
                Assert.Empty(deleteButtons);
                _output.WriteLine("Verified all Delete buttons were removed");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error: {ex.Message}");
                throw;
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}
