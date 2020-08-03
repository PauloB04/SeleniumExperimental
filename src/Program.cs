using System;
using dotenv.net;
using dotenv.net.Utilities;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExpTestProject.src.Others;
using SeleniumExpTestProject.src.Workers;

namespace SeleniumExpTestProject
{
    class Program
    {
        private static IWebDriver driverGlobal = null;

        static void Main(string[] args)
        {
            try
            {
                DotEnvConfig();
                Misc.StartProgramMsg();

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
                Console.WriteLine("Reached final code block. Press any key to exit.");
                Console.ReadKey();
            }
        }

        private static async Task RunCode(IWebDriver driver)//Runs the core of the app with help from other libraries
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
            Data.SetWaitGlobal(wait);
            driverGlobal = InitiateDriver(driver);
            var isRegisteredAndLoggedIn = await Core.AttemptToRegisterCredentials(driver, Data.GetTestCaseUsername(), Data.GetTestCasePassword());
            
            if (!isRegisteredAndLoggedIn)
            {
                var isLoggedIn = await Core.AttemptToLogIn(driver, Data.GetTestCaseUsername(), Data.GetTestCasePassword());

                if (!isLoggedIn)
                {
                    Misc.Beep();
                    Console.WriteLine("An error occured while attempting to log in");
                    Console.WriteLine("Press any key to acknowledge error & quit.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }

            Console.WriteLine("Successfully logged in");

            Data.SetSecret(string.Empty, Data.GetTestCaseUsername());
            var wasSecretSubmitted = await Core.SubmitSecret(driver, Data.GetSecret());

            Misc.Beep();
            Console.WriteLine("Press any key to quit test window.");
            Console.ReadKey();
        }

        #region Setup

        private static IWebDriver InitiateDriver(IWebDriver driver)//Starts up the webDriver needed to open a browser window
        {
            driver.Url = Data.webAppUrl;
            driver.Navigate();
            driver.Manage().Window.Maximize();
            return driver;
        }

        private static void DotEnvConfig()//Configures DotEnv files to find & recognize environment variables
        {
            DotEnv.Config();
            EnvReader envReader = new EnvReader();
            Data.SetTestCaseUsername(envReader.GetStringValue("USERNAME"));
            Data.SetTestCasePassword(envReader.GetStringValue("PASSWORD"));
        }
        #endregion
    }
}
