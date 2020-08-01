using System;
using dotenv.net;
using dotenv.net.Utilities;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExpTestProject.src.Others;

namespace SeleniumExpTestProject
{
    class Program
    {
        private static IWebDriver driverGlobal = null;
        private static WebDriverWait waitGlobal = null;

        static void Main(string[] args)
        {
            try
            {
                DotEnvConfig();
                using (IWebDriver driver = new ChromeDriver())
                {
                    try
                    {
                        RunCode(driver);
                    }
                    catch (Exception e)
                    {
                        Misc.HandleException(e, Data.excMsgUsingDriverBlock);
                    }
                }
            }
            catch (Exception e)
            {
                Misc.HandleException(e, Data.excMsgMain);
            }
            finally
            {
                driverGlobal.Quit();
                Console.WriteLine("Reached Finally");
                Console.ReadKey();
            }
        }

        private static async Task RunCode(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
            waitGlobal = wait;
            driverGlobal = InitiateDriver(driver);
            var isRegisteredAndLoggedIn = await AttemptToRegisterCredentials(driver, Data.GetTestCaseUsername(), Data.GetTestCasePassword());

            //TODO: If user is already registered, then attempt log in
            if (!isRegisteredAndLoggedIn)
            {
                var isLoggedIn = await AttemptToLogIn(driver, Data.GetTestCaseUsername(), Data.GetTestCasePassword());

                if (isLoggedIn)
                {
                    //SubmitSecret();
                    Console.WriteLine("Successfully logged in");
                }
            }

            //Submit a secret

            Console.ReadKey();
            Thread.Sleep(10000);
        }

        private static IWebDriver InitiateDriver(IWebDriver driver)
        {
            driver.Url = Data.webAppUrl;
            driver.Navigate();
            driver.Manage().Window.Maximize();
            return driver;
        }

        private static async Task<bool> AttemptToRegisterCredentials(IWebDriver driver, string username, string password)
        {
            var registerCondition = By.CssSelector("button");

            driver.FindElement(By.ClassName("btn-light")).Click();

            var result = await IsTextPresentInElement(driver, registerCondition, Data.registerButtonText);

            if (result)
            {
                Thread.Sleep(Data.threadSleepTime);//TODO: result returns true but the system still attemps to sendKeys() a tad too fast, there might be a cleaner way around?

                if (Misc.IsStringValid(username) && Misc.IsStringValid(password))
                {
                    driver.FindElement(By.Name("username")).SendKeys(username);
                    driver.FindElement(By.Name("password")).SendKeys(password);
                }
                else
                {
                    Console.WriteLine($"{nameof(username)} or {nameof(password)} isn't a valid string");
                }
            }

            Console.WriteLine($"Press any key to proceed with Button click");
            //Console.ReadKey();

            driver.FindElement(By.CssSelector("button")).Click();

            var isLoggedIn = await IsLoggedIn(driver);

            return isLoggedIn;
        }

        private static async Task<bool> AttemptToLogIn(IWebDriver driver, string username, string password)
        {
            if (Misc.IsStringValid(username) && Misc.IsStringValid(password))
            {
                var loginLocator = By.CssSelector("h1");
                driver.Url = Data.webAppUrl + Data.loginRoute;
                driver.Navigate();
                var result = await IsTextPresentInElement(driver, loginLocator, Data.loginText);

                if (result)
                {
                    Thread.Sleep(Data.threadSleepTime);
                    driver.FindElement(By.Name(nameof(username))).SendKeys(username);
                    driver.FindElement(By.Name(nameof(password))).SendKeys(password);
                    driver.FindElement(By.CssSelector("button")).Click();
                }
            }
            return await IsLoggedIn(driver);
        }

        #region Utilities
        
        private static async Task<bool> IsLoggedIn(IWebDriver driver)
        {
            try
            {
                var element = By.LinkText("Log Out");
                var result = waitGlobal.Until(ExpectedConditions.ElementExists(element));
                Console.WriteLine($"{nameof(result)}: {result}");

                if (result != null)
                {
                    Console.WriteLine($"{nameof(element)} found: {element}");
                    return true;
                }
            }
            catch (WebDriverTimeoutException e)
            {
                Misc.HandleException(e, Data.excMsgIsLoggedIn);
            }
            catch (Exception e)
            {
                Misc.HandleException(e, Data.excMsgIsLoggedIn);
            }

            return false;
        }

        private static async Task<bool> IsTextPresentInElement(IWebDriver driver, By elementLocator, string text)
        {
            try
            {
                var result = waitGlobal.Until(ExpectedConditions.TextToBePresentInElement(driver.FindElement(elementLocator), text));
                Console.WriteLine($"{nameof(result)}: {result}");

                return result;
            } catch (WebDriverTimeoutException e)
            {
                Misc.HandleException(e, Data.excMsgIsTextPresent);
            }
            catch (Exception e)
            {
                Misc.HandleException(e, Data.excMsgIsTextPresent);
            }

            return false;
        }

        private static void DotEnvConfig()
        {
            DotEnv.Config();
            EnvReader envReader = new EnvReader();
            Data.SetTestCaseUsername(envReader.GetStringValue("USERNAME"));
            Data.SetTestCasePassword(envReader.GetStringValue("PASSWORD"));
        }
        #endregion
    }
}
