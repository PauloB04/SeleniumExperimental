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
                DotEnv.Config();
                EnvReader envReader = new EnvReader();
                Data.SetTestCaseUsername(envReader.GetStringValue("USERNAME"));
                Data.SetTestCasePassword(envReader.GetStringValue("PASSWORD"));

                using (IWebDriver driver = new ChromeDriver())
                {
                    try
                    {
                        RunCode(driver);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Exception found insde 'using' block:");
                        Console.WriteLine($"Exception message: {e.Message}");
                        Console.WriteLine($"Exception code trace: {e.StackTrace}");
                    }
                }
            }
            catch (Exception e)
            {
                Misc.HandleException(e, );
                Console.WriteLine($"Exception found:");
                Console.WriteLine($"Exception message: {e.Message}");
                Console.WriteLine($"Exception code trace: {e.StackTrace}");
            }
            finally
            {
                //driverGlobal.Close();
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
            await AttemptToRegisterCredentials(driver, Data.GetTestCaseUsername(), Data.GetTestCasePassword());
            
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

            var result = await IsTextPresentInElement(driver, waitGlobal, registerCondition, Data.registerButtonText);
            
            if (result)
            {
                Thread.Sleep(400);//TODO: result returns true but the system still attemps to sendKeys() a tad too fast, there might be a cleaner way around?

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
            Console.ReadKey();
            driver.FindElement(By.CssSelector("button")).Click();
            return true;
        }

        private static async Task<bool> IsTextPresentInElement(IWebDriver driver, WebDriverWait wait, By elementLocator, string text)
        {
            var result = wait.Until(ExpectedConditions.TextToBePresentInElement(driver.FindElement(elementLocator), text));
            Console.WriteLine($"{nameof(result)}: {result}");
            return result;
        }
    }
}
