﻿using System;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SeleniumExpTestProject.src.Others;

namespace SeleniumExpTestProject.src.Workers
{
    class Core: Utilities
    {
        internal static async Task<bool> AttemptToRegisterCredentials(IWebDriver driver, string username, string password)
        {
            var registerCondition = By.CssSelector(Data.btnCssSelector);

            driver.FindElement(By.ClassName("btn-light")).Click();

            var result = await IsTextPresentInElement(driver, registerCondition, Data.registerButtonText);

            if (result)
            {
                Thread.Sleep(Data.threadSleepTime);//TODO: result returns true but the system still attemps to sendKeys() a tad too fast,
                                                    //there might be a cleaner way around?

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

            driver.FindElement(By.CssSelector("button")).Click();

            var isLoggedIn = await IsLoggedIn(driver);

            return isLoggedIn;
        }

        internal static async Task<bool> AttemptToLogIn(IWebDriver driver, string username, string password)
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

        internal static async Task<bool> SubmitSecret(IWebDriver driver, string secret)
        {
            var wasSubmissionSuccessful = false;
            var submitElementLocator = By.CssSelector("p");
            var elementWithSubmittedSecret = By.CssSelector("p");
            var submitButton = driver.FindElement(By.LinkText(Data.submitSecretBtnText));

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
    }
}