using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace SeleniumExpTestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (IWebDriver driver = new ChromeDriver())
                {
                    try
                    {
                        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                        driver.Url = "https://pbarbeiro-secrets.herokuapp.com/";
                        driver.Navigate();
                        driver.Manage().Window.Maximize();
                        driver.FindElement(By.ClassName("btn-light")).Click();
                        Thread.Sleep(2000);
                        
                        driver.FindElement(By.Name("username")).SendKeys("Bobo@boborinco");
                        driver.FindElement(By.Name("password")).SendKeys("Loco123&");

                        Console.WriteLine($"Press any key to proceed with Button press");
                        Console.ReadKey();
                        driver.FindElement(By.CssSelector("button")).Click();
                        Thread.Sleep(10000);

                        /*
                         TODO: 
                         - Write dynamic test to identify when a user has already been registered, and if so, use the credentials for login
                         - Write code to post a 'Secret' with the credentials used for login
                         - Find how to use WebDriverWait to wait for specific events instead of using Thread.Sleep
                         - Modify Web app code to provide feed back when user has already been registered
                         - Modify Web app code to provide feedback when a user tries to login/register with empty strings (?)
                         */
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
                Console.WriteLine("Reched Finally");
                Console.ReadKey();
            }
           
        }
    }
}
