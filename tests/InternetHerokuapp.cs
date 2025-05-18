using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SeleniumTestDemo.Tests
{
    public class WebDriverFixture : IDisposable
    {
        public IWebDriver Driver { get; private set; }

        public WebDriverFixture()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            Driver = new ChromeDriver(options);
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public void Dispose()
        {
            Driver?.Quit();
            Driver?.Dispose();
        }
    }

    public class InternetHerokuappTests : IClassFixture<WebDriverFixture>
    {
        private readonly IWebDriver _driver;
        private readonly ITestOutputHelper _output;
        private readonly WebDriverWait _wait;
        private const string BaseUrl = "https://the-internet.herokuapp.com/";

        public InternetHerokuappTests(WebDriverFixture fixture, ITestOutputHelper output)
        {
            _driver = fixture.Driver;
            _output = output;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public void InternetHerokuappNavLinks_Work()
        {
            try
            {
                _output.WriteLine($"\n📘 Nav Test Log - {DateTime.Now}\n");
                _driver.Navigate().GoToUrl(BaseUrl);
                
                var navItems = _driver.FindElements(By.CssSelector("ul li"));
                _output.WriteLine($"\nFound {navItems.Count} <li> elements in the <ul>.");
                
                foreach (var item in navItems)
                {
                    _output.WriteLine($"  - {item.Text}");
                }
                
                Assert.True(navItems.Count > 0, "No navigation items found");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error in navigation test: {ex.Message}");
                throw;
            }
        }

        [Fact]
        public void AddRemoveElements_Work()
        {
            try
            {
                _output.WriteLine($"\n📘 Add/Remove Elements Test Log - {DateTime.Now}\n");
                _driver.Navigate().GoToUrl($"{BaseUrl}add_remove_elements/");
                
                var addButton = _wait.Until(d => d.FindElement(By.CssSelector("button[onclick='addElement()']")));
                _output.WriteLine("Found Add Element button");
                addButton.Click();
                _output.WriteLine("Clicked Add Element button");
                
                var deleteButton = _wait.Until(d => d.FindElement(By.CssSelector("button.added-manually")));
                _output.WriteLine("Found Delete button");
                deleteButton.Click();
                _output.WriteLine("Clicked Delete button");
                
                Thread.Sleep(1000);
                var deleteButtons = _driver.FindElements(By.CssSelector("button.added-manually"));
                Assert.Empty(deleteButtons);
                _output.WriteLine("Verified Delete button was removed");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error in add/remove elements test: {ex.Message}");
                throw;
            }
        }

        [Fact]
        public void AddRemoveMultipleElements_Work()
        {
            try
            {
                _output.WriteLine($"\n📘 Add/Remove Multiple Elements Test Log - {DateTime.Now}\n");
                _driver.Navigate().GoToUrl($"{BaseUrl}add_remove_elements/");
                
                var addButton = _wait.Until(d => d.FindElement(By.CssSelector("button[onclick='addElement()']")));
                _output.WriteLine("Found Add Element button");
                
                // Add 5 elements
                for (int i = 1; i <= 5; i++)
                {
                    addButton.Click();
                    Thread.Sleep(500); // Small delay between clicks
                    _output.WriteLine($"Added element {i}");
                }
                
                var deleteButtons = _driver.FindElements(By.CssSelector("button.added-manually"));
                Assert.Equal(5, deleteButtons.Count);
                _output.WriteLine("Verified 5 Delete buttons are present");
                
                // Delete all buttons
                for (int i = 5; i >= 1; i--)
                {
                    var button = _wait.Until(d => d.FindElement(By.CssSelector("button.added-manually")));
                    button.Click();
                    Thread.Sleep(500); // Small delay between clicks
                    _output.WriteLine($"Deleted element {i}");
                }
                
                deleteButtons = _driver.FindElements(By.CssSelector("button.added-manually"));
                Assert.Empty(deleteButtons);
                _output.WriteLine("Verified all Delete buttons were removed");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error in add/remove multiple elements test: {ex.Message}");
                throw;
            }
        }

        [Fact]
        public void BasicAuth_Work()
        {
            try
            {
                _output.WriteLine($"\n📘 Testing Auth Test Log - {DateTime.Now}\n");
                string username = "admin";
                string password = "admin";
                string baseUrl = $"https://{username}:{password}@the-internet.herokuapp.com/basic_auth/";
                
                _driver.Navigate().GoToUrl(baseUrl);
                
                var successMessage = _wait.Until(d => d.FindElement(By.CssSelector("p")));
                Assert.Contains("Congratulations!", successMessage.Text);
                _output.WriteLine("Successfully authenticated and found success message");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error in basic auth test: {ex.Message}");
                throw;
            }
        }
    }
}
