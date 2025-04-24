using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public static class HomeNetNavTest
{
    public static async Task Run()
    {
        string baseUrl = "https://www.homenetauto.com";
        string navSelector = "ul#header-menu li a";
        string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "nav-test-log.txt");

        File.WriteAllText(logFilePath, $"📘 Nav Test Log - {DateTime.Now}\n\n");

        IWebDriver driver = new ChromeDriver();

        try
        {
            driver.Navigate().GoToUrl(baseUrl);
            await Task.Delay(2000);

            var navLinks = driver.FindElements(By.CssSelector(navSelector)).ToList();

            Console.WriteLine($"🔍 Found {navLinks.Count} nav links\n");
            File.AppendAllText(logFilePath, $"🔍 Found {navLinks.Count} nav links\n\n");

            for (int i = 0; i < navLinks.Count; i++)
            {
                try
                {
                    var linkText = navLinks[i].Text.Trim();
                    var href = navLinks[i].GetAttribute("href");

                    if (string.IsNullOrWhiteSpace(href) || href == baseUrl || href == baseUrl + "/")
                    {
                        string skipMsg = $"⏭️ Skipping: {linkText} ({href})\n";
                        Console.WriteLine(skipMsg);
                        File.AppendAllText(logFilePath, skipMsg);
                        continue;
                    }

                    Console.WriteLine($"➡️ Clicking: {linkText} ({href})");
                    File.AppendAllText(logFilePath, $"➡️ Clicking: {linkText} ({href})\n");

                    navLinks[i].Click();
                    await Task.Delay(2000);

                    string currentUrl = driver.Url;
                    string currentTitle = driver.Title;
                    bool urlMatched = currentUrl.TrimEnd('/') == href.TrimEnd('/');
                    bool hasTitle = !string.IsNullOrWhiteSpace(currentTitle);

                    if (urlMatched && hasTitle)
                    {
                        string passMsg = $"✅ '{linkText}' page loaded successfully.\n";
                        Console.WriteLine(passMsg);
                        File.AppendAllText(logFilePath, passMsg);
                    }
                    else
                    {
                        string warnMsg = $"⚠️ '{linkText}' did not navigate to a new page (URL: {currentUrl}).\n";
                        Console.WriteLine(warnMsg);
                        File.AppendAllText(logFilePath, warnMsg);
                    }

                    driver.Navigate().Back();
                    await Task.Delay(2000);
                    navLinks = driver.FindElements(By.CssSelector(navSelector)).ToList();
                }
                catch (StaleElementReferenceException)
                {
                    string staleMsg = $"♻️ Skipped due to stale element.\n";
                    Console.WriteLine(staleMsg);
                    File.AppendAllText(logFilePath, staleMsg);
                    navLinks = driver.FindElements(By.CssSelector(navSelector)).ToList();
                }
                catch (Exception ex)
                {
                    string errorMsg = $"❌ Error: {ex.Message}\n";
                    Console.WriteLine(errorMsg);
                    File.AppendAllText(logFilePath, errorMsg);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Critical failure: {ex.Message}");
        }
        finally
        {
            driver.Quit();
            Console.WriteLine($"📄 Log written to: {logFilePath}\n");
        }
    }
}
