using System;
using dotenv.net;
using dotenv.net.Utilities;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumExpTestProject
{
    class Program
    {
        private const string webappUrl = "https://pbarbeiro-secrets.herokuapp.com/";
        private static string testCaseUsername = string.Empty;
        private static string testCasePassword = string.Empty;
        private static IWebDriver driverGlobal = null;
        private static WebDriverWait waitGlobal = null;
        
        static void Main(string[] args)
        {
            try
            {
                DotEnv.Config();
                EnvReader envReader = new EnvReader();
                testCaseUsername = envReader.GetStringValue("USERNAME");
                testCasePassword = envReader.GetStringValue("PASSWORD");

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
                Console.WriteLine($"Exception found:");
                Console.WriteLine($"Exception message: {e.Message}");
                Console.WriteLine($"Exception code trace: {e.StackTrace}");
            }
            finally
            {
                driverGlobal.Close();
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
            await AttemptToRegisterCredentials(driver, testCaseUsername, testCasePassword);

            //TODO: Write code to check and handle for element on the page that follows user registry.
            //TODO: Find way to check for http response instead?
            //TODO: Console window isn't showing up, why?

            Console.ReadKey();
            Thread.Sleep(10000);

            /*
             TODO: 
             - Write dynamic test to identify when a user has already been registered, and if so, use the credentials for login
             - Write code to post a 'Secret' with the credentials used for login
             - Modify Web app code to provide feed back when user has already been registered
             - Modify Web app code to provide feedback when a user tries to login/register with empty strings (?) 
             - Organize code
             - Organize TODOs lol
             - Assign appropriate return values for methods
             */
        }

        private static IWebDriver InitiateDriver(IWebDriver driver)
        {
            driver.Url = webappUrl;
            driver.Navigate();
            driver.Manage().Window.Maximize();
            return driver;
        }

        private static async Task<bool> AttemptToRegisterCredentials(IWebDriver driver, string username, string password)
        {
            var registerCondition = By.CssSelector("h1");
            var registerText = "Register";

            //TODO: Check to see if strings aren't empty
            //TODO: Instead of checking for 'Register', check if password input field is present

            driver.FindElement(By.ClassName("btn-light")).Click();
            var result = await IsTextPresentInElement(driver, waitGlobal, registerCondition, registerText);
            
            if (result)
            {
                Thread.Sleep(300);//TODO: result returns true but there system still attemps to sendKeys() a tad too fast, there might be a cleaner way around?
                driver.FindElement(By.Name("username")).SendKeys(username);
                driver.FindElement(By.Name("password")).SendKeys(password);
            }
            
            Console.WriteLine($"Press any key to proceed with Button press");
            Console.ReadKey();
            driver.FindElement(By.CssSelector("button")).Click();
            return true;
        }

        private static async Task<bool> IsTextPresentInElement(IWebDriver driver, WebDriverWait wait, By elementLocator, string text)
        {
           var result = wait.Until(ExpectedConditions.TextToBePresentInElement(driver.FindElement(elementLocator), text));
           //TODO: ExpectedConditions is deprecated, find and substitute with newer version
            return result;
        }
    }
}
