using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Almostengr.FalconPiMonitor
{
    public class CheckOnlineTests : BaseTest
    {
        private IWebDriver driver;

        [OneTimeSetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();

// #if RELEASE
            options.AddArgument("--headless");
// #endif

            driver = new ChromeDriver(options);
        }

        [Test]
        public void CheckReachable()
        {
            // act
            driver.Navigate().GoToUrl(websiteUrl);

            // assert
            Assert.IsTrue(driver.Title.Contains("falconpi"));
        }

        [Test]
        public void CheckSchedulerStatus()
        {
            // act
            driver.Navigate().GoToUrl(websiteUrl);
            string statusText = driver.FindElement(By.Id("schedulerStatus")).Text.ToLower();

            TestContext.Progress.WriteLine("Schedule Status: {0}", statusText);

            // assert
            Assert.IsTrue(statusText.Contains("playing"));
        }

        [Test]
        public void TemperatureCheck()
        {
            // act
            driver.Navigate().GoToUrl(websiteUrl);
            string temperature = driver.FindElement(By.Id("sensorTable")).Text;
            temperature = temperature.Replace("CPU: ", "").Replace("C", "");
            int firstDigit = Convert.ToInt16(temperature.Substring(0,1));

            TestContext.Progress.WriteLine("Temperature: {0}", temperature);

            // assert
            Assert.AreNotEqual(firstDigit, 6);
            Assert.AreNotEqual(firstDigit, 7);
            Assert.AreNotEqual(firstDigit, 8);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }
    }
}