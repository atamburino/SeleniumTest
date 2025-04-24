using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading.Tasks;

public static class OpenAIChatTest
{
    public static async Task Run()
    {
        string appUrl = "https://anointed-ai.vercel.app/chat";
        string testMessage = "Who is Jesus?";

        ChromeOptions options = new ChromeOptions();
        IWebDriver driver = new ChromeDriver(options);

        try
        {
            driver.Navigate().GoToUrl(appUrl);
            await Task.Delay(1); // to avoid CS1998 warning

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            // Wait for chat input to appear and type message
            IWebElement input = wait.Until(d =>
                d.FindElement(By.CssSelector("input[placeholder='Type your message here...']")));

            input.SendKeys(testMessage);
            Console.WriteLine("✅ Input field located and test message sent.");

            // Wait for the Send button to become enabled
            wait.Until(d =>
            {
                var btn = d.FindElements(By.CssSelector("button.chakra-button.css-b48sry")).FirstOrDefault();
                return btn != null && btn.Enabled;
            });

            IWebElement sendBtn = driver.FindElement(By.CssSelector("button.chakra-button.css-b48sry"));
            sendBtn.Click();
            Console.WriteLine("✅ Send button clicked.");

            // Give the chat UI a moment to process and render the response
            await Task.Delay(3000); // 3 seconds — adjust as needed

            // Wait for new messages to appear
            wait.Until(d => d.FindElements(By.CssSelector("p.chakra-text")).Any());

            var responseElements = driver.FindElements(By.CssSelector("p.chakra-text"));
            Console.WriteLine($"🔍 Found {responseElements.Count} <p.chakra-text> elements.");

            // Debug log all message text
            foreach (var el in responseElements)
            {
                Console.WriteLine($"→ {el.Text}");
            }

            // Extract response directly after the sent message
            var messages = responseElements.Select(e => e.Text.Trim()).ToList();
            int sentIndex = messages.FindIndex(m => m.Equals(testMessage, StringComparison.OrdinalIgnoreCase));

            string latestResponse = (sentIndex != -1 && sentIndex + 1 < messages.Count)
                ? messages[sentIndex + 1]
                : null;

            if (!string.IsNullOrWhiteSpace(latestResponse))
            {
                Console.WriteLine($"🧠 OpenAI Response: {latestResponse}");
                Console.WriteLine("✅ OpenAI response successfully rendered in UI.");
            }
            else
            {
                Console.WriteLine("❌ Could not find OpenAI response after your message.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"🚨 Chat test failed: {ex.Message}");
        }
        finally
        {
            driver.Quit();
        }
    }
}
