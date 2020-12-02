using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static Almostengr.FalconPiMonitor.Logger;

namespace Almostengr.FalconPiMonitor
{
    public class PiMonitor
    {
        private IWebDriver driver;
        private const string websiteUrl = "http://falconpi/";
        private bool errorReported = false;
        private int failCounter = 0;

        public void PerformChecks()
        {
            Random random = new Random();
            int waitDelay = random.Next(3, 10); // * 10000;

            try
            {
                StartBrowser();
                CheckReachable();
                CheckSchedulerStatus();
                TemperatureCheck();
                NoRestartRequired();

                errorReported = false; // reset flag if previously triggered
            }
            catch (ElementNotVisibleException ex)
            {
                ReportError($"Element was not visible on page. {ex}");
            }
            catch (WebDriverException ex)
            {
                ReportError($"A Webdriver Exception occurred. {ex}");
            }

            CloseBrowser();

            LogMessage($"Waiting {waitDelay} minutes between attempts");
            Thread.Sleep(TimeSpan.FromMinutes(waitDelay));
        }

        private void StartBrowser()
        {
            LogMessage("Starting browser");

            ChromeOptions options = new ChromeOptions();

#if RELEASE
            options.AddArgument("--headless");
#endif

            driver = new ChromeDriver(options);
        }

        private void CheckReachable()
        {
            driver.Navigate().GoToUrl(websiteUrl);

            if (driver.Title.Contains("falconpi") == false)
            {
                ReportError("Falcon Pi Player not reachable");
            }
        }

        private void CheckSchedulerStatus()
        {
            driver.Navigate().GoToUrl(websiteUrl);
            string statusText = driver.FindElement(By.Id("schedulerStatus")).Text.ToLower();

            LogMessage($"Schedule Status: {statusText}");

            if (statusText.Contains("playing") == false && statusText.Contains("manual") == true)
            {
                ReportError();
                StopPlaybackGracefully();
            }
        }

        private void TemperatureCheck()
        {
            driver.Navigate().GoToUrl(websiteUrl);
            string temperature = driver.FindElement(By.Id("sensorTable")).Text
                .Replace("CPU: ", "").Replace("C", "");
            int firstDigit = Convert.ToInt16(temperature.Substring(0, 1));

            LogMessage($"Temperature: {temperature}");

            if (firstDigit == 6 || firstDigit == 7 || firstDigit == 8 || firstDigit == 9)
            {
                ReportError($"Temperature is higher than expected level. {temperature}");
            }
        }

        private void NoRestartRequired()
        {
            driver.Navigate().GoToUrl(websiteUrl);
            string restartFlagVisble = driver.FindElement(By.Id("restartFlag")).GetAttribute("style").ToLower();

            if (restartFlagVisble.Contains("display: none;") == false)
            {
                ReportError("FPP needs to restart");
                RestartFalconPlayer();
            }
        }

        private void CloseBrowser()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }

        private void ReportError(string message = "Error occurred")
        {
            if (errorReported)
                return; // exit if error has already been reported

            LogMessage(message);

            errorReported = true;
            failCounter++;
        }

        private void RestartFalconPlayer()
        {
            LogMessage("Restarting Falcon Player");
#if RELEASE
            driver.FindElement(By.Id("btnRestartFPPD")).Click();
#endif
        }

        private void StopPlaybackGracefully()
        {
            LogMessage("Stopping gracefully");

#if RELEASE
            driver.FindElement(By.Id("btnStopGracefully")).Click();
#endif
        }
    }
}