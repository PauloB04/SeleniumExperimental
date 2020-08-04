using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SeleniumExpTestProject.src.Others;
using System.Collections.Generic;

namespace SeleniumExpTestProject.src.Workers
{
    class Core: Utilities
    {
        internal static async Task<bool> AttemptToRegisterCredentials(IWebDriver driver, string username, string password)//Attempts to register a user with 
        {                                                                                                                 //username & password credentials
            var registerCondition = By.CssSelector(Data.btnCssSelector);

            driver.FindElement(By.ClassName("btn-light")).Click();

            var result = await IsTextPresentInElement(driver, registerCondition, Data.registerButtonText);

            if (result)
            {
                Misc.Sleep(Data.threadSleepTime);//TODO: result returns true but the system still attemps to sendKeys() a tad too fast,
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

        internal static async Task<bool> AttemptToLogIn(IWebDriver driver, string username, string password)//Attempts to log in with username & password
        {
            if (Misc.IsStringValid(username) && Misc.IsStringValid(password))
            {
                var loginLocator = By.CssSelector("h1");
                driver.Url = Data.webAppUrl + Data.loginRoute;
                driver.Navigate();
                var result = await IsTextPresentInElement(driver, loginLocator, Data.loginText);

                if (result)
                {
                    Misc.Sleep(Data.threadSleepTime);
                    driver.FindElement(By.Name(nameof(username))).SendKeys(username);
                    driver.FindElement(By.Name(nameof(password))).SendKeys(password);
                    driver.FindElement(By.CssSelector(Data.btnCssSelector)).Click();
                }
            }
            return await IsLoggedIn(driver);
        }

        internal static async Task<bool> SubmitSecret(IWebDriver driver, string secret)//Attempts to submit/post a 'secret' on the website * 
        {
            var wasSubmissionSuccessful = false;
            var submitElementLocator = By.CssSelector("p");
            var elementWithSubmittedSecret = By.CssSelector("p");
            var submitButton = driver.FindElement(By.LinkText(Data.submitSecretBtnText));

            Console.WriteLine($"{nameof(submitButton)} result: {submitButton}");
            Misc.Sleep(.6);
            submitButton.Click();

            var isElementPresent = await IsTextPresentInElement(driver, submitElementLocator, Data.submitPageTextId);

            if (isElementPresent)
            {
                Misc.Sleep(Data.threadSleepTime);
                driver.FindElement(By.Name(nameof(secret))).Clear();
                driver.FindElement(By.Name(nameof(secret))).SendKeys(secret);
                driver.FindElement(By.CssSelector(Data.btnCssSelector)).Click();

                var hasSecretBeenSubmitted = await IsLoggedIn(driver);

                if (hasSecretBeenSubmitted)//This part of the code is supposed to ensure the submitted secret is on the actual displayed list
                {
                    Misc.Sleep(Data.threadSleepTime);

                    var resultsForCssSelectorP = driver.FindElements(By.CssSelector("p"));

                    for (int i=0; i<resultsForCssSelectorP.Count;i++)
                    {
                        Misc.Space();
                        Console.WriteLine($"Text in P element: **{resultsForCssSelectorP[i].Text}**");

                        if (resultsForCssSelectorP[i].Text == Data.secret)
                        {
                            wasSubmissionSuccessful = true;
                            Misc.Space();
                            Console.WriteLine(Data.secretSubmitSuccessful);
                            return wasSubmissionSuccessful;
                        }
                    }
                }
            }

            Console.WriteLine(Data.secretSubmitUnSuccessful);
            return wasSubmissionSuccessful;
        }
        
        internal static Dictionary<string, string> CheckCredentials()//Sets credentials to a default value in case it hasn't been properly set
        {
            var credentials = new Dictionary<string, string>();
            var username = Data.GetTestCaseUsername();
            var password = Data.GetTestCasePassword();
            credentials.Add(username, password);

            if (Misc.IsStringNullOr(username) || Misc.IsStringNullOr(password))
            {
                Data.SetTestCaseUsername("John@Doe");
                username = Data.GetTestCaseUsername();

                Data.SetTestCasePassword("RandomPassword123^");
                password = Data.GetTestCasePassword();

                credentials.Clear();
                credentials.Add(username, password);
            }

            return credentials;
        }
    }
}