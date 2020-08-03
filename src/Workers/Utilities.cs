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

        protected static async Task<bool> IsTextPresentInElement(IWebDriver driver, By elementLocator, string text)//Checks if text is present in a page element
        {
            try
            {
                var waitGlobal = Data.GetWaitGlobal();
                var result = waitGlobal.Until(ExpectedConditions.TextToBePresentInElement(driver.FindElement(elementLocator), text));
                Console.WriteLine($"{nameof(result)}: {result}");

                return result;
            }
            catch (WebDriverTimeoutException e)
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
    }
}
