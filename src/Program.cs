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
            
            if (!isRegisteredAndLoggedIn)
            {
                var isLoggedIn = await AttemptToLogIn(driver, Data.GetTestCaseUsername(), Data.GetTestCasePassword());

                if (!isLoggedIn)
                {
                    Console.Beep();
                    Console.WriteLine("An error occured while attempting to log in");
                    Console.WriteLine("Press any key to acknowledge error & quit.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }

            Console.WriteLine("Successfully logged in");

            Data.SetSecret(string.Empty, Data.GetTestCaseUsername());
            var wasSecretSubmitted = await SubmitSecret(driver, Data.GetSecret());

            Console.WriteLine("Press any key to proceed");
            Console.ReadKey();
            Console.WriteLine("Thread will be sleeping for 10000 ms");
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
            var registerCondition = By.CssSelector(Data.btnCssSelector);

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
                    driver.FindElement(By.CssSelector(Data.btnCssSelector)).Click();
                }
            }
            return await IsLoggedIn(driver);
        }

        private static async Task<bool> SubmitSecret(IWebDriver driver, string secret)
        {
            var wasSubmissionSuccessful = false;
            var submitElementLocator = By.CssSelector("p");
            var elementWithSubmittedSecret = By.CssSelector("p");
            //Thread.Sleep(Data.threadSleepTime);
            var submitButton = driver.FindElement(By.LinkText(Data.submitSecretBtnText)); //TODO: this action isnt happening somehow

            Console.WriteLine($"{nameof(submitButton)} result: {submitButton}");
            Thread.Sleep(600);
            submitButton.Click();

            var isElementPresent = await IsTextPresentInElement(driver, submitElementLocator, Data.submitPageTextId);

            if (isElementPresent)
            {
                Thread.Sleep(Data.threadSleepTime);
                driver.FindElement(By.Name(nameof(secret))).Clear();
                driver.FindElement(By.Name(nameof(secret))).SendKeys(secret);
                driver.FindElement(By.CssSelector(Data.btnCssSelector)).Click();

                var hasSecretBeenSubmitted = await IsLoggedIn(driver);

                /*if (hasSecretBeenSubmitted)
                {
                    Thread.Sleep(Data.threadSleepTime);

                    var isSecretPresent = await IsTextPresentInElement(driver, elementWithSubmittedSecret, secret);

                    wasSubmissionSuccessful = isSecretPresent;
                }*/
                wasSubmissionSuccessful = hasSecretBeenSubmitted;
            }

            return wasSubmissionSuccessful;
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
                Console.WriteLine($"Expected text: {text}");
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
