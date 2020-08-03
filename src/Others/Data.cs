﻿using OpenQA.Selenium.Support.UI;

namespace SeleniumExpTestProject.src.Others
{
    internal static class Data
    {
        private static string testCaseUsername = string.Empty;
        private static string testCasePassword = string.Empty;
        private static WebDriverWait waitGlobal = null;
        internal const string webAppUrl = "https://pbarbeiro-secrets.herokuapp.com/";
        internal static string secret = $"This secret was generated automatically as a test for user '{testCaseUsername}'";
        internal const int threadSleepTime = 400;
        internal const string btnCssSelector = "button";
        internal const string submitPageTextId = "Don't keep your secrets, share them anonymously!";
        internal const string submitSecretBtnText = "Submit a Secret";
        internal const string loginRoute = "login";
        internal const string loginText = "Login";
        internal const string registerButtonText = "Register";
        internal const string excMsgUsingDriverBlock = "Exception found insde 'using' block:";
        internal const string excMsgMain = "Exception found:";
        internal const string excMsgIsLoggedIn = "Exception found while checking logged in state";
        internal const string excMsgIsTextPresent = "Exception found while checking if text is present in element:";

        #region Set
        internal static void SetTestCaseUsername(string username)
        {
            testCaseUsername = username;
        }

        internal static void SetTestCasePassword(string password)
        {
            testCasePassword = password;
        }

        internal static void SetSecret(string _secret, string _testCaseUsername)
        {
            secret = Misc.IsStringNullOr(_secret) ?
            $"This secret was generated automatically as a test for user '{_testCaseUsername}'" : _secret + " " + _testCaseUsername;
        }

        internal static void SetWaitGlobal(WebDriverWait wait)
        {
            waitGlobal = wait;
        }
        #endregion

        #region Get
        internal static string GetTestCaseUsername()
        {
            return testCaseUsername;
        }

        internal static string GetTestCasePassword()
        {
            return testCasePassword;
        }

        internal static string GetSecret()
        {
            return secret;
        }

        internal static WebDriverWait GetWaitGlobal()
        {
            return waitGlobal;
        }
        #endregion
    }
}
