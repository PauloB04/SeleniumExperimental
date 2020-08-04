using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SeleniumExpTestProject.src.Others;
using SeleniumExtras.WaitHelpers;

namespace SeleniumExpTestProject.src.Workers
{
    class Utilities
    {
        protected static async Task<bool> IsLoggedIn(IWebDriver driver)//Checks if app has logged into SecretsBlog website
        {
            try
            {
                var waitGlobal = Data.GetWaitGlobal();
                var element = By.LinkText("Log Out");
                var result = waitGlobal.Until(ExpectedConditions.ElementExists(element));
                Console.WriteLine($"{nameof(result)}: {result}");

                DisplayResults(element, result) ;

                if (result != null)
                {
                    Misc.Space();
                    Console.WriteLine($"{nameof(element)} found: {element}");
                    Misc.Space();
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

        protected static async Task<bool> IsTextPresentInElement(IWebDriver driver, By elementLocator, string text)//Checks if text is present in a page element
        {
            try
            {
                var waitGlobal = Data.GetWaitGlobal();
                var result = waitGlobal.Until(ExpectedConditions.TextToBePresentInElement(driver.FindElement(elementLocator), text));
                DisplayResults(elementLocator, text, result);

                return result;
            }
            catch (WebDriverTimeoutException e)
            {
                Misc.Space();
                Console.WriteLine($"Expected text: **{text}**");
                Misc.Space();
                Misc.HandleException(e, Data.excMsgIsTextPresent);
                Misc.Space();
            }
            catch (Exception e)
            {
                Misc.HandleException(e, Data.excMsgIsTextPresent);
            }

            return false;
        }

        private static void DisplayResults(By elementLocator, string text, bool result)
        {
            Misc.Space();
            Console.WriteLine($"*** Showing results for Results for --{nameof(IsTextPresentInElement)}-- ***");
            Misc.Space();
            Console.WriteLine($"Looking for text **{text}** in element -- {elementLocator} --");
            Misc.Space();
            Console.WriteLine($"{nameof(result)}: !{result}! for text **{text}**");
            Misc.Space();
        }

        private static void DisplayResults(By elementLocator, IWebElement result)
        {
            Misc.Space();
            Console.WriteLine($"*** Showing results for Results for -- {nameof(IsLoggedIn)} -- ***");
            Misc.Space();
            Console.WriteLine($"Looking for element -- {elementLocator} --");
            Misc.Space();
            Console.WriteLine($"{nameof(result)}: !{result}! for element **{elementLocator}**");
            Misc.Space();
        }
    }
}
