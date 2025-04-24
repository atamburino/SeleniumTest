using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public static class WeatherHybridTest
{
    public static async Task Run()
    {
        string city = "Orlando";
        string apiKey = "YOUR_API_KEY"; // Replace with real key
        string weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=imperial";
        string appUrl = "http://localhost:5000"; // Replace with your weather site

        IWebDriver driver = new ChromeDriver();

        try
        {
            driver.Navigate().GoToUrl(appUrl);
            await Task.Delay(2000);

            IWebElement fetchButton = driver.FindElement(By.Id("get-weather"));
            fetchButton.Click();
            await Task.Delay(2000);

            IWebElement uiTempElement = driver.FindElement(By.Id("weather-temp"));
            string uiTempText = uiTempElement.Text;
            float uiTemp = float.Parse(uiTempText.Split(' ')[1].Replace("°F", ""));

            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(weatherUrl);
            string json = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(json);
            float apiTemp = doc.RootElement.GetProperty("main").GetProperty("temp").GetSingle();

            Console.WriteLine($"🌤️ UI Temp: {uiTemp}°F | API Temp: {apiTemp}°F");

            float margin = 2.0f;
            if (Math.Abs(uiTemp - apiTemp) <= margin)
            {
                Console.WriteLine("✅ Weather displayed correctly on UI!\n");
            }
            else
            {
                Console.WriteLine("❌ Mismatch between UI and API temp.\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"🚨 Weather Test Error: {ex.Message}");
        }
        finally
        {
            driver.Quit();
        }
    }
}
